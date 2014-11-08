using System;
using System.Collections.Generic;
using System.Messaging;
using System.Diagnostics;
using System.Threading;
 
namespace HAKGERSoft {
 
    /// <summary>
    /// Sends and receives .NET objects between processes using MSMQ
    /// </summary>
    public class MSMQDispatcher {
        readonly string QueuePath;
        readonly MessageQueue Queue;
        Cursor Location;
        List<Type> RegisteredTypes;
        SynchronizationContext Context;
 
        /// <summary>
        /// Occurs when new message arrive - use RegisterType() before
        /// </summary>
        public event EventHandler<DispatcherEventArgs> OnReceived;
 
        /// <summary>
        /// Creates new MSMQDispatcher instance
        /// </summary>
        /// <param name="queueName">Common name of the MSMQ queue that is used between processes</param>
        public MSMQDispatcher(string queueName) {
            QueuePath=GetQueuePath(queueName);
            if(!MessageQueue.Exists(QueuePath)) {
                Debug.WriteLine("Provided queue does not exist, creating new one");
                MessageQueue.Create(QueuePath);
            }
            Queue=new MessageQueue(QueuePath);
            RegisteredTypes=new List<Type>();
        }
 
        /// <summary>
        /// Sends an object 
        /// </summary>
        /// <typeparam name="T">Type of object to be sent</typeparam>
        /// <param name="label">Additional label of the message</param>
        /// <param name="obj">Object to sent</param>
        public void Send<T>(string label,T obj) {
            Queue.Send(obj,label);
        }
 
        /// <summary>
        /// Clears all previous messages from the queue
        /// </summary>
        public void Purge() {
            Queue.Purge();
        }
 
        /// <summary>
        /// Start listen MSMQ for new messages - use this function if you want to receive data
        /// </summary>
        public void StartListener() {
            if(Location!=null)
                throw new InvalidOperationException("Listener already started");
            Context=SynchronizationContext.Current;
            if(Context==null)
                Context=new SynchronizationContext();
            Location=GetCurrentLocarion();
            BeginPeek(PeekAction.Current);
        }
 
        /// <summary>
        /// Register type that should be received
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void RegisterType<T>() {
            RegisteredTypes.Add(typeof(T));
        }
 
        string GetQueuePath(string queueName) {
            return string.Format(@".\private$\{0}",queueName);
        }
 
        void BeginPeek(PeekAction peekAction) {
            Queue.BeginPeek(MessageQueue.InfiniteTimeout,Location,peekAction,null,QueueAsyncCallBack);
        }
 
        void QueueAsyncCallBack(IAsyncResult asyncResult) {
            Message message= Queue.EndPeek(asyncResult);
            message.Formatter=new XmlMessageFormatter(RegisteredTypes.ToArray());
            object body=null;
            try {
                body=message.Body;
            } catch(InvalidOperationException ex) {
                Debug.WriteLine(ex.ToString());
            }
            if(body!=null)
                RaiseReceived(message.Label,body);
            BeginPeek(PeekAction.Next);
        }
 
        void RaiseReceived(string label,object message) {
            PostCallback<DispatcherEventArgs>(OnReceived,new DispatcherEventArgs(label,message));
        }
 
        void PostCallback<T>(EventHandler<T> handler,T args) where T:EventArgs {
            if(handler == null)
                return;
            Context.Post(new SendOrPostCallback((state) => { 
                handler(this,args); 
            }),null);
        }
 
        Cursor GetCurrentLocarion() {
            var cursor=Queue.CreateCursor();
            try {
                Queue.Peek(TimeSpan.Zero,cursor,PeekAction.Current);
                while(true) {
                    Queue.Peek(TimeSpan.Zero,cursor,PeekAction.Next);
                }
            } catch(MessageQueueException ex) {
                if(ex.MessageQueueErrorCode!=MessageQueueErrorCode.IOTimeout) {
                    throw;
                }
            }
            return cursor;
        }
    }
 
    public class DispatcherEventArgs:EventArgs {
        public readonly string MessageLabel;
        public readonly object MessageObject;
 
        public DispatcherEventArgs(string label,object message) {
            MessageLabel=label;
            MessageObject=message;
        }
    }
}