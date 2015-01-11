// AForge Math Library
// AForge.NET framework
// http://www.aforgenet.com/framework/
//
// Copyright © Andrew Kirillov, 2005-2009
// andrew.kirillov@aforgenet.com
//
// FFT idea from Exocortex.DSP library
// http://www.exocortex.org/dsp/
//

﻿  using System;
namespace AForge.Math
{

﻿  /// <summary>
﻿  /// Fourier transformation.
﻿  /// </summary>
﻿  /// 
﻿  /// <remarks>The class implements one dimensional and two dimensional
﻿  /// Discrete and Fast Fourier Transformation.</remarks>
﻿  /// 
﻿  public static class FourierTransform
﻿  {
﻿  ﻿  /// <summary>
﻿  ﻿  /// Fourier transformation direction.
﻿  ﻿  /// </summary>
﻿  ﻿  public enum Direction
﻿  ﻿  {
﻿  ﻿  ﻿  /// <summary>
﻿  ﻿  ﻿  /// Forward direction of Fourier transformation.
﻿  ﻿  ﻿  /// </summary>
﻿  ﻿  ﻿  Forward = 1,

﻿  ﻿  ﻿  /// <summary>
﻿  ﻿  ﻿  /// Backward direction of Fourier transformation.
﻿  ﻿  ﻿  /// </summary>
﻿  ﻿  ﻿  Backward = -1
﻿  ﻿  };﻿  ﻿  

﻿  ﻿  /// <summary>
﻿  ﻿  /// One dimensional Discrete Fourier Transform.
﻿  ﻿  /// </summary>
﻿  ﻿  /// 
﻿  ﻿  /// <param name="data">Data to transform.</param>
﻿  ﻿  /// <param name="direction">Transformation direction.</param>
﻿  ﻿  /// 
      public static void DFT(Complex[] data, Direction direction)
﻿  ﻿  {
﻿  ﻿  ﻿  System.Int32﻿  ﻿  ﻿  n = data.Length;
﻿  ﻿  ﻿  Double﻿ arg, cos, sin;
﻿  ﻿  ﻿  Complex[]﻿  dst = new Complex[n];

﻿  ﻿  ﻿  // for each destination element
﻿  ﻿  ﻿  for (int i = 0; i < n; i++)
﻿  ﻿  ﻿  {
﻿  ﻿  ﻿  ﻿  dst[i] = Complex.Zero;

﻿  ﻿  ﻿  ﻿  arg = -(int)direction * 2.0 * System.Math.PI * (double)i / (double)n;

﻿  ﻿  ﻿  ﻿  // sum source elements
﻿  ﻿  ﻿  ﻿  for (int j = 0; j < n; j++)
﻿  ﻿  ﻿  ﻿  {
﻿  ﻿  ﻿  ﻿  ﻿  cos = System.Math.Cos(j * arg);
﻿  ﻿  ﻿  ﻿  ﻿  sin = System.Math.Sin(j * arg);

﻿  ﻿  ﻿  ﻿  ﻿  dst[i].Re += (data[j].Re * cos - data[j].Im * sin);
﻿  ﻿  ﻿  ﻿  ﻿  dst[i].Im += (data[j].Re * sin + data[j].Im * cos);
﻿  ﻿  ﻿  ﻿  }
﻿  ﻿  ﻿  }

﻿  ﻿  ﻿  // copy elements
         if (direction == Direction.Forward)
﻿  ﻿  ﻿  {
﻿  ﻿  ﻿  ﻿  // devide also for forward transform
﻿  ﻿  ﻿  ﻿  for (int i = 0; i < n; i++)
﻿  ﻿  ﻿  ﻿  {
﻿  ﻿  ﻿  ﻿  ﻿  data[i].Re = dst[i].Re / n;
﻿  ﻿  ﻿  ﻿  ﻿  data[i].Im = dst[i].Im / n;
﻿  ﻿  ﻿  ﻿  }
﻿  ﻿  ﻿  }
﻿  ﻿  ﻿  else
﻿  ﻿  ﻿  {
﻿  ﻿  ﻿  ﻿  for (int i = 0; i < n; i++)
﻿  ﻿  ﻿  ﻿  {
﻿  ﻿  ﻿  ﻿  ﻿  data[i].Re = dst[i].Re;
﻿  ﻿  ﻿  ﻿  ﻿  data[i].Im = dst[i].Im;
﻿  ﻿  ﻿  ﻿  }
﻿  ﻿  ﻿  }
﻿  ﻿  }

﻿  ﻿  /// <summary>
﻿  ﻿  /// Two dimensional Discrete Fourier Transform.
﻿  ﻿  /// </summary>
﻿  ﻿  /// 
﻿  ﻿  /// <param name="data">Data to transform.</param>
﻿  ﻿  /// <param name="direction">Transformation direction.</param>
﻿  ﻿  /// 
      public static void DFT2(Complex[,] data, Direction direction)
﻿  ﻿  {
﻿  ﻿  ﻿  Int32﻿  ﻿  ﻿  n = data.GetLength(0);﻿  // rows
﻿  ﻿  ﻿  Int﻿32  ﻿  ﻿  m = data.GetLength(1);﻿  // columns
﻿  ﻿  ﻿  Double﻿  ﻿  arg, cos, sin;
﻿  ﻿  ﻿  Complex[]﻿  dst = new Complex[System.Math.Max(n, m)];

﻿  ﻿  ﻿  // process rows
﻿  ﻿  ﻿  for (int i = 0; i < n; i++)
﻿  ﻿  ﻿  {
﻿  ﻿  ﻿  ﻿  for (int j = 0; j < m; j++)
﻿  ﻿  ﻿  ﻿  {
﻿  ﻿  ﻿  ﻿  ﻿  dst[j] = Complex.Zero;

﻿  ﻿  ﻿  ﻿  ﻿  arg = -(int)direction * 2.0 * System.Math.PI * (double)j / (double)m;

﻿  ﻿  ﻿  ﻿  ﻿  // sum source elements
﻿  ﻿  ﻿  ﻿  ﻿  for (int k = 0; k < m; k++)
﻿  ﻿  ﻿  ﻿  ﻿  {
﻿  ﻿  ﻿  ﻿  ﻿  ﻿  cos = System.Math.Cos(k * arg);
﻿  ﻿  ﻿  ﻿  ﻿  ﻿  sin = System.Math.Sin(k * arg);

﻿  ﻿  ﻿  ﻿  ﻿  ﻿  dst[j].Re += (data[i, k].Re * cos - data[i, k].Im * sin);
﻿  ﻿  ﻿  ﻿  ﻿  ﻿  dst[j].Im += (data[i, k].Re * sin + data[i, k].Im * cos);
﻿  ﻿  ﻿  ﻿  ﻿  }
﻿  ﻿  ﻿  ﻿  }

﻿  ﻿  ﻿  ﻿  // copy elements
            if (direction == Direction.Forward)
﻿  ﻿  ﻿  ﻿  {
﻿  ﻿  ﻿  ﻿  ﻿  // devide also for forward transform
﻿  ﻿  ﻿  ﻿  ﻿  for (int j = 0; j < m; j++)
﻿  ﻿  ﻿  ﻿  ﻿  {
﻿  ﻿  ﻿  ﻿  ﻿  ﻿  data[i, j].Re = dst[j].Re / m;
﻿  ﻿  ﻿  ﻿  ﻿  ﻿  data[i, j].Im = dst[j].Im / m;
﻿  ﻿  ﻿  ﻿  ﻿  }
﻿  ﻿  ﻿  ﻿  }
﻿  ﻿  ﻿  ﻿  else
﻿  ﻿  ﻿  ﻿  {
﻿  ﻿  ﻿  ﻿  ﻿  for (int j = 0; j < m; j++)
﻿  ﻿  ﻿  ﻿  ﻿  {
﻿  ﻿  ﻿  ﻿  ﻿  ﻿  data[i, j].Re = dst[j].Re;
﻿  ﻿  ﻿  ﻿  ﻿  ﻿  data[i, j].Im = dst[j].Im;
﻿  ﻿  ﻿  ﻿  ﻿  }
﻿  ﻿  ﻿  ﻿  }
﻿  ﻿  ﻿  }

﻿  ﻿  ﻿  // process columns
﻿  ﻿  ﻿  for (int j = 0; j < m; j++)
﻿  ﻿  ﻿  {
﻿  ﻿  ﻿  ﻿  for (int i = 0; i < n; i++)
﻿  ﻿  ﻿  ﻿  {
﻿  ﻿  ﻿  ﻿  ﻿  dst[i] = Complex.Zero;

﻿  ﻿  ﻿  ﻿  ﻿  arg = -(int)direction * 2.0 * System.Math.PI * (double)i / (double)n;

﻿  ﻿  ﻿  ﻿  ﻿  // sum source elements
﻿  ﻿  ﻿  ﻿  ﻿  for (int k = 0; k < n; k++)
﻿  ﻿  ﻿  ﻿  ﻿  {
﻿  ﻿  ﻿  ﻿  ﻿  ﻿  cos = System.Math.Cos(k * arg);
﻿  ﻿  ﻿  ﻿  ﻿  ﻿  sin = System.Math.Sin(k * arg);

﻿  ﻿  ﻿  ﻿  ﻿  ﻿  dst[i].Re += (data[k, j].Re * cos - data[k, j].Im * sin);
﻿  ﻿  ﻿  ﻿  ﻿  ﻿  dst[i].Im += (data[k, j].Re * sin + data[k, j].Im * cos);
﻿  ﻿  ﻿  ﻿  ﻿  }
﻿  ﻿  ﻿  ﻿  }

﻿  ﻿  ﻿  ﻿  // copy elements
            if (direction == Direction.Forward)
﻿  ﻿  ﻿  ﻿  {
﻿  ﻿  ﻿  ﻿  ﻿  // devide also for forward transform
﻿  ﻿  ﻿  ﻿  ﻿  for (int i = 0; i < n; i++)
﻿  ﻿  ﻿  ﻿  ﻿  {
﻿  ﻿  ﻿  ﻿  ﻿  ﻿  data[i, j].Re = dst[i].Re / n;
﻿  ﻿  ﻿  ﻿  ﻿  ﻿  data[i, j].Im = dst[i].Im / n;
﻿  ﻿  ﻿  ﻿  ﻿  }
﻿  ﻿  ﻿  ﻿  }
﻿  ﻿  ﻿  ﻿  else
﻿  ﻿  ﻿  ﻿  {
﻿  ﻿  ﻿  ﻿  ﻿  for (int i = 0; i < n; i++)
﻿  ﻿  ﻿  ﻿  ﻿  {
﻿  ﻿  ﻿  ﻿  ﻿  ﻿  data[i, j].Re = dst[i].Re;
﻿  ﻿  ﻿  ﻿  ﻿  ﻿  data[i, j].Im = dst[i].Im;
﻿  ﻿  ﻿  ﻿  ﻿  }
﻿  ﻿  ﻿  ﻿  }
﻿  ﻿  ﻿  }
﻿  ﻿  }


﻿  ﻿  /// <summary>
﻿  ﻿  /// One dimensional Fast Fourier Transform.
﻿  ﻿  /// </summary>
﻿  ﻿  /// 
﻿  ﻿  /// <param name="data">Data to transform.</param>
﻿  ﻿  /// <param name="direction">Transformation direction.</param>
      /// 
      /// <remarks><para><note>The method accepts <paramref name="data"/> array of 2<sup>n</sup> size
      /// only, where <b>n</b> may vary in the [1, 14] range.</note></para></remarks>
      /// 
      /// <exception cref="ArgumentException">Incorrect data length.</exception>
      /// 
      public static void FFT(Complex[] data, Direction direction)
﻿  ﻿  {
﻿  ﻿  ﻿  Int32﻿  ﻿  n = data.Length;
﻿  ﻿  ﻿  Int﻿32  ﻿  m = Tools.Log2(n);

﻿  ﻿  ﻿  // reorder data first
﻿  ﻿  ﻿  ReorderData(data);

﻿  ﻿  ﻿  // compute FFT
﻿  ﻿  ﻿  int tn = 1, tm;

﻿  ﻿  ﻿  for (int k = 1; k <= m; k++)
﻿  ﻿  ﻿  {
﻿  ﻿  ﻿  ﻿  Complex[] rotation = FourierTransform.GetComplexRotation(k, direction);

﻿  ﻿  ﻿  ﻿  tm = tn;
﻿  ﻿  ﻿  ﻿  tn <<= 1;

﻿  ﻿  ﻿  ﻿  for (int i = 0; i < tm; i++)
﻿  ﻿  ﻿  ﻿  {
﻿  ﻿  ﻿  ﻿  ﻿  Complex t = rotation[i];

﻿  ﻿  ﻿  ﻿  ﻿  for (int even = i; even < n; even += tn)
﻿  ﻿  ﻿  ﻿  ﻿  {
﻿  ﻿  ﻿  ﻿  ﻿  ﻿  var odd = even + tm;
﻿  ﻿  ﻿  ﻿  ﻿  ﻿  Complex﻿ ce = data[even];
﻿  ﻿  ﻿  ﻿  ﻿  ﻿  Complex﻿ co = data[odd];

﻿  ﻿  ﻿  ﻿  ﻿  ﻿  Double﻿ tr = co.Re * t.Re - co.Im * t.Im;
﻿  ﻿  ﻿  ﻿  ﻿  ﻿  Double﻿ ti = co.Re * t.Im + co.Im * t.Re;

﻿  ﻿  ﻿  ﻿  ﻿  ﻿  data[even].Re += tr;
﻿  ﻿  ﻿  ﻿  ﻿  ﻿  data[even].Im += ti;

﻿  ﻿  ﻿  ﻿  ﻿  ﻿  data[odd].Re = ce.Re - tr;
﻿  ﻿  ﻿  ﻿  ﻿  ﻿  data[odd].Im = ce.Im - ti;
﻿  ﻿  ﻿  ﻿  ﻿  }
﻿  ﻿  ﻿  ﻿  }
﻿  ﻿  ﻿  }

         if (direction == Direction.Forward) 
﻿  ﻿  ﻿  {
﻿  ﻿  ﻿  ﻿  for (int i = 0; i < n; i++) 
﻿  ﻿  ﻿  ﻿  {
﻿  ﻿  ﻿  ﻿  ﻿  data[i].Re /= (double)n;
﻿  ﻿  ﻿  ﻿  ﻿  data[i].Im /= (double)n;
﻿  ﻿  ﻿  ﻿  }
﻿  ﻿  ﻿  }
﻿  ﻿  }

﻿  ﻿  /// <summary>
﻿  ﻿  /// Two dimensional Fast Fourier Transform.
﻿  ﻿  /// </summary>
﻿  ﻿  /// 
﻿  ﻿  /// <param name="data">Data to transform.</param>
﻿  ﻿  /// <param name="direction">Transformation direction.</param>
﻿  ﻿  /// 
      /// <remarks><para><note>The method accepts <paramref name="data"/> array of 2<sup>n</sup> size
      /// only in each dimension, where <b>n</b> may vary in the [1, 14] range. For example, 16x16 array
      /// is valid, but 15x15 is not.</note></para></remarks>
      /// 
      /// <exception cref="ArgumentException">Incorrect data length.</exception>
      /// 
      public static void FFT2(Complex[,] data, Direction direction)
﻿  ﻿  {
﻿  ﻿  ﻿  int k = data.GetLength(0);
﻿  ﻿  ﻿  int n = data.GetLength(1);

﻿  ﻿  ﻿  // check data size
﻿  ﻿  ﻿  if (
﻿  ﻿  ﻿  ﻿  (!Tools.IsPowerOf2(k)) ||
﻿  ﻿  ﻿  ﻿  (!Tools.IsPowerOf2(n)) ||
﻿  ﻿  ﻿  ﻿  (k < minLength) || (k > maxLength) ||
﻿  ﻿  ﻿  ﻿  (n < minLength) || (n > maxLength)
﻿  ﻿  ﻿  ﻿  )
﻿  ﻿  ﻿  {
﻿  ﻿  ﻿  ﻿  throw new ArgumentException("Incorrect data length.");
﻿  ﻿  ﻿  }

﻿  ﻿  ﻿  // process rows
﻿  ﻿  ﻿  Complex[]﻿  row = new Complex[n];

﻿  ﻿  ﻿  for (int i = 0; i < k; i++)
﻿  ﻿  ﻿  {
﻿  ﻿  ﻿  ﻿  // copy row
﻿  ﻿  ﻿  ﻿  for (int j = 0; j < n; j++)
﻿  ﻿  ﻿  ﻿  ﻿  row[j] = data[i, j];
﻿  ﻿  ﻿  ﻿  // transform it
﻿  ﻿  ﻿  ﻿  FourierTransform.FFT(row, direction);
﻿  ﻿  ﻿  ﻿  // copy back
﻿  ﻿  ﻿  ﻿  for (int j = 0; j < n; j++)
﻿  ﻿  ﻿  ﻿  ﻿  data[i, j] = row[j];
﻿  ﻿  ﻿  }

﻿  ﻿  ﻿  // process columns
﻿  ﻿  ﻿  Complex[]﻿  col = new Complex[k];

﻿  ﻿  ﻿  for (int j = 0; j < n; j++)
﻿  ﻿  ﻿  {
﻿  ﻿  ﻿  ﻿  // copy column
﻿  ﻿  ﻿  ﻿  for (int i = 0; i < k; i++)
﻿  ﻿  ﻿  ﻿  ﻿  col[i] = data[i, j];
﻿  ﻿  ﻿  ﻿  // transform it
﻿  ﻿  ﻿  ﻿  FourierTransform.FFT(col, direction);
﻿  ﻿  ﻿  ﻿  // copy back
﻿  ﻿  ﻿  ﻿  for (int i = 0; i < k; i++)
﻿  ﻿  ﻿  ﻿  ﻿  data[i, j] = col[i];
﻿  ﻿  ﻿  }
﻿  ﻿  }

﻿  ﻿  #region Private Region

﻿  ﻿  private const Int32﻿  ﻿  minLength﻿ = 2;
﻿  ﻿  private const Int32﻿  ﻿  maxLength﻿ = 16384;
﻿  ﻿  private const Int32﻿  ﻿  minBits﻿  ﻿  = 1;
﻿  ﻿  private const Int32﻿  ﻿  maxBits﻿  ﻿  = 14;
﻿  ﻿  private static int[][]﻿  reversedBits = new int[maxBits][];
﻿  ﻿  private static Complex[,][]﻿  complexRotation = new Complex[maxBits, 2][];

﻿  ﻿  // Get array, indicating which data members should be swapped before FFT
﻿  ﻿  private static int[] GetReversedBits(int numberOfBits)
﻿  ﻿  {
﻿  ﻿  ﻿  if ((numberOfBits < minBits) || (numberOfBits > maxBits))
﻿  ﻿  ﻿  ﻿  throw new ArgumentOutOfRangeException();

﻿  ﻿  ﻿  // check if the array is already calculated
﻿  ﻿  ﻿  if (reversedBits[numberOfBits - 1] == null)
﻿  ﻿  ﻿  {
﻿  ﻿  ﻿  ﻿  Int32﻿  ﻿  n = Tools.Pow2(numberOfBits);
﻿  ﻿  ﻿  ﻿  int[]﻿  rBits = new int[n];

﻿  ﻿  ﻿  ﻿  // calculate the array
﻿  ﻿  ﻿  ﻿  for (int i = 0; i < n; i++)
﻿  ﻿  ﻿  ﻿  {
﻿  ﻿  ﻿  ﻿  ﻿  int oldBits = i;
﻿  ﻿  ﻿  ﻿  ﻿  int newBits = 0;

﻿  ﻿  ﻿  ﻿  ﻿  for (int j = 0; j < numberOfBits; j++)
﻿  ﻿  ﻿  ﻿  ﻿  {
﻿  ﻿  ﻿  ﻿  ﻿  ﻿  newBits = (newBits << 1) | (oldBits & 1);
﻿  ﻿  ﻿  ﻿  ﻿  ﻿  oldBits = (oldBits >> 1);
﻿  ﻿  ﻿  ﻿  ﻿  }
﻿  ﻿  ﻿  ﻿  ﻿  rBits[i] = newBits;
﻿  ﻿  ﻿  ﻿  }
﻿  ﻿  ﻿  ﻿  reversedBits[numberOfBits - 1] = rBits;
﻿  ﻿  ﻿  }
﻿  ﻿  ﻿  return reversedBits[numberOfBits - 1];
﻿  ﻿  }

﻿  ﻿  // Get rotation of complex number
      private static Complex[] GetComplexRotation(int numberOfBits, Direction direction)
﻿  ﻿  {
          int directionIndex = (direction == Direction.Forward) ? 0 : 1;

﻿  ﻿  ﻿  // check if the array is already calculated
﻿  ﻿  ﻿  if (complexRotation[numberOfBits - 1, directionIndex] == null)
﻿  ﻿  ﻿  {
             
﻿  ﻿  ﻿  ﻿  Int32﻿  ﻿  ﻿  n = 1 << (numberOfBits - 1);
﻿  ﻿  ﻿  ﻿  Double﻿  ﻿  uR = 1.0;
﻿  ﻿  ﻿  ﻿  Double﻿  ﻿  uI = 0.0;
﻿  ﻿  ﻿  ﻿  Double﻿  ﻿  angle = System.Math.PI / n * (int)direction;
﻿  ﻿  ﻿  ﻿  Double﻿  ﻿  wR = System.Math.Cos(angle);
﻿  ﻿  ﻿  ﻿  Double﻿  ﻿  wI = System.Math.Sin(angle);
﻿  ﻿  ﻿  ﻿  Double﻿  ﻿  t;
﻿  ﻿  ﻿  ﻿  Complex[]﻿  rotation = new Complex[n];

﻿  ﻿  ﻿  ﻿  for (int i = 0; i < n; i++)
﻿  ﻿  ﻿  ﻿  {
﻿  ﻿  ﻿  ﻿  ﻿  rotation[i] = new Complex(uR, uI);
﻿  ﻿  ﻿  ﻿  ﻿  t = uR * wI + uI * wR;
﻿  ﻿  ﻿  ﻿  ﻿  uR = uR * wR - uI * wI;
﻿  ﻿  ﻿  ﻿  ﻿  uI = t;
﻿  ﻿  ﻿  ﻿  }

﻿  ﻿  ﻿  ﻿  complexRotation[numberOfBits - 1, directionIndex] = rotation;
﻿  ﻿  ﻿  }
﻿  ﻿  ﻿  return complexRotation[numberOfBits - 1, directionIndex];
﻿  ﻿  }

﻿  ﻿  // Reorder data for FFT using
﻿  ﻿  private static void ReorderData(Complex[] data)
﻿  ﻿  {
﻿  ﻿  ﻿  int len = data.Length;

﻿  ﻿  ﻿  // check data length
﻿  ﻿  ﻿  if ((len < minLength) || (len > maxLength) || (!Tools.IsPowerOf2(len)))
﻿  ﻿  ﻿  ﻿  throw new ArgumentException("Incorrect data length.");

﻿  ﻿  ﻿  int[] rBits = GetReversedBits(Tools.Log2(len));

﻿  ﻿  ﻿  for (int i = 0; i < len; i++)
﻿  ﻿  ﻿  {
﻿  ﻿  ﻿  ﻿  int s = rBits[i];

﻿  ﻿  ﻿  ﻿  if (s > i)
﻿  ﻿  ﻿  ﻿  {
﻿  ﻿  ﻿  ﻿  ﻿  Complex t = data[i];
﻿  ﻿  ﻿  ﻿  ﻿  data[i] = data[s];
﻿  ﻿  ﻿  ﻿  ﻿  data[s] = t;
﻿  ﻿  ﻿  ﻿  }
﻿  ﻿  ﻿  }
﻿  ﻿  }

﻿  ﻿  #endregion
﻿  }
}

namespace AForge.Math
{
    using System;
    using System.Text.RegularExpressions;
    using System.Runtime.Serialization;

    /// <summary>
    /// Complex number wrapper class.
    /// </summary>
    /// 
    /// <remarks><para>The class encapsulates complex number and provides
    /// set of different operators to manipulate it, lake adding, subtractio,
    /// multiplication, etc.</para>
    /// 
    /// <para>Sample usage:</para>
    /// <code>
    /// // define two complex numbers
    /// Complex c1 = new Complex( 3, 9 );
    /// Complex c2 = new Complex( 8, 3 );
    /// // sum
    /// Complex s1 = Complex.Add( c1, c2 );
    /// Complex s2 = c1 + c2;
    /// Complex s3 = c1 + 5;
    /// // difference
    /// Complex d1 = Complex.Subtract( c1, c2 );
    /// Complex d2 = c1 - c2;
    /// Complex d3 = c1 - 2;
    /// </code>
    /// </remarks>
    /// 
    public struct Complex : ICloneable, ISerializable
    {
        /// <summary>
        /// Real part of the complex number.
        /// </summary>
        public double Re;

        /// <summary>
        /// Imaginary part of the complex number.
        /// </summary>
        public double Im;

        /// <summary>
        ///  A double-precision complex number that represents zero.
        /// </summary>
        public static readonly Complex Zero = new Complex(0, 0);

        /// <summary>
        ///  A double-precision complex number that represents one.
        /// </summary>
        public static readonly Complex One = new Complex(1, 0);

        /// <summary>
        ///  A double-precision complex number that represents the squere root of (-1).
        /// </summary>
        public static readonly Complex I = new Complex(0, 1);

        /// <summary>
        /// Magnitude value of the complex number.
        /// </summary>
        /// 
        /// <remarks><para>Magnitude of the complex number, which equals to <b>Sqrt( Re * Re + Im * Im )</b>.</para></remarks>
        /// 
        public double Magnitude
        {
            get { return System.Math.Sqrt(Re * Re + Im * Im); }
        }

        /// <summary>
        /// Phase value of the complex number.
        /// </summary>
        /// 
        /// <remarks><para>Phase of the complex number, which equals to <b>Atan( Im / Re )</b>.</para></remarks>
        /// 
        public double Phase
        {
            get { return System.Math.Atan2(Im, Re); }
        }

        /// <summary>
        /// Squared magnitude value of the complex number.
        /// </summary>
        public double SquaredMagnitude
        {
            get { return (Re * Re + Im * Im); }
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="Complex"/> class.
        /// </summary>
        /// 
        /// <param name="re">Real part.</param>
        /// <param name="im">Imaginary part.</param>
        /// 
        public Complex(double re, double im)
        {
            this.Re = re;
            this.Im = im;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Complex"/> class.
        /// </summary>
        /// 
        /// <param name="c">Source complex number.</param>
        /// 
        public Complex(Complex c)
        {
            this.Re = c.Re;
            this.Im = c.Im;
        }

        /// <summary>
        /// Adds two complex numbers.
        /// </summary>
        /// 
        /// <param name="a">A <see cref="Complex"/> instance.</param>
        /// <param name="b">A <see cref="Complex"/> instance.</param>
        /// 
        /// <returns>Returns new <see cref="Complex"/> instance containing the sum of specified
        /// complex numbers.</returns>
        /// 
        public static Complex Add(Complex a, Complex b)
        {
            return new Complex(a.Re + b.Re, a.Im + b.Im);
        }

        /// <summary>
        /// Adds scalar value to a complex number.
        /// </summary>
        /// 
        /// <param name="a">A <see cref="Complex"/> instance.</param>
        /// <param name="s">A scalar value.</param>
        /// 
        /// <returns>Returns new <see cref="Complex"/> instance containing the sum of specified
        /// complex number and scalar value.</returns>
        /// 
        public static Complex Add(Complex a, double s)
        {
            return new Complex(a.Re + s, a.Im);
        }

        /// <summary>
        /// Adds two complex numbers and puts the result into the third complex number.
        /// </summary>
        /// 
        /// <param name="a">A <see cref="Complex"/> instance.</param>
        /// <param name="b">A <see cref="Complex"/> instance.</param>
        /// <param name="result">A <see cref="Complex"/> instance to hold the result.</param>
        /// 
        public static void Add(Complex a, Complex b, ref Complex result)
        {
            result.Re = a.Re + b.Re;
            result.Im = a.Im + b.Im;
        }

        /// <summary>
        /// Adds scalar value to a complex number and puts the result into another complex number.
        /// </summary>
        /// 
        /// <param name="a">A <see cref="Complex"/> instance.</param>
        /// <param name="s">A scalar value.</param>
        /// <param name="result">A <see cref="Complex"/> instance to hold the result.</param>
        /// 
        public static void Add(Complex a, double s, ref Complex result)
        {
            result.Re = a.Re + s;
            result.Im = a.Im;
        }

        /// <summary>
        /// Subtracts one complex number from another.
        /// </summary>
        /// 
        /// <param name="a">A <see cref="Complex"/> instance to subtract from.</param>
        /// <param name="b">A <see cref="Complex"/> instance to be subtracted.</param>
        /// 
        /// <returns>Returns new <see cref="Complex"/> instance containing the subtraction result (<b>a - b</b>).</returns>
        /// 
        public static Complex Subtract(Complex a, Complex b)
        {
            return new Complex(a.Re - b.Re, a.Im - b.Im);
        }

        /// <summary>
        /// Subtracts a scalar from a complex number.
        /// </summary>
        /// 
        /// <param name="a">A <see cref="Complex"/> instance to subtract from.</param>
        /// <param name="s">A scalar value to be subtracted.</param>
        /// 
        /// <returns>Returns new <see cref="Complex"/> instance containing the subtraction result (<b>a - s</b>).</returns>
        /// 
        public static Complex Subtract(Complex a, double s)
        {
            return new Complex(a.Re - s, a.Im);
        }

        /// <summary>
        /// Subtracts a complex number from a scalar value.
        /// </summary>
        /// 
        /// <param name="s">A scalar value to subtract from.</param>
        /// <param name="a">A <see cref="Complex"/> instance to be subtracted.</param>
        /// 
        /// <returns>Returns new <see cref="Complex"/> instance containing the subtraction result (<b>s - a</b>).</returns>
        /// 
        public static Complex Subtract(double s, Complex a)
        {
            return new Complex(s - a.Re, a.Im);
        }

        /// <summary>
        /// Subtracts one complex number from another and puts the result in the third complex number.
        /// </summary>
        /// 
        /// <param name="a">A <see cref="Complex"/> instance to subtract from.</param>
        /// <param name="b">A <see cref="Complex"/> instance to be subtracted.</param>
        /// <param name="result">A <see cref="Complex"/> instance to hold the result.</param>
        /// 
        public static void Subtract(Complex a, Complex b, ref Complex result)
        {
            result.Re = a.Re - b.Re;
            result.Im = a.Im - b.Im;
        }

        /// <summary>
        /// Subtracts a scalar value from a complex number and puts the result into another complex number.
        /// </summary>
        /// 
        /// <param name="a">A <see cref="Complex"/> instance to subtract from.</param>
        /// <param name="s">A scalar value to be subtracted.</param>
        /// <param name="result">A <see cref="Complex"/> instance to hold the result.</param>
        /// 
        public static void Subtract(Complex a, double s, ref Complex result)
        {
            result.Re = a.Re - s;
            result.Im = a.Im;
        }

        /// <summary>
        /// Subtracts a complex number from a scalar value and puts the result into another complex number.
        /// </summary>
        /// 
        /// <param name="s">A scalar value to subtract from.</param>
        /// <param name="a">A <see cref="Complex"/> instance to be subtracted.</param>
        /// <param name="result">A <see cref="Complex"/> instance to hold the result.</param>
        /// 
        public static void Subtract(double s, Complex a, ref Complex result)
        {
            result.Re = s - a.Re;
            result.Im = a.Im;
        }

        /// <summary>
        /// Multiplies two complex numbers.
        /// </summary>
        /// 
        /// <param name="a">A <see cref="Complex"/> instance.</param>
        /// <param name="b">A <see cref="Complex"/> instance.</param>
        /// 
        /// <returns>Returns new <see cref="Complex"/> instance containing the result of multiplication.</returns>
        /// 
        public static Complex Multiply(Complex a, Complex b)
        {
            // (x + yi)(u + vi) = (xu  yv) + (xv + yu)i. 
            double aRe = a.Re, aIm = a.Im;
            double bRe = b.Re, bIm = b.Im;

            return new Complex(aRe * bRe - aIm * bIm, aRe * bIm + aIm * bRe);
        }

        /// <summary>
        /// Multiplies a complex number by a scalar value.
        /// </summary>
        /// 
        /// <param name="a">A <see cref="Complex"/> instance.</param>
        /// <param name="s">A scalar value.</param>
        /// 
        /// <returns>Returns new <see cref="Complex"/> instance containing the result of multiplication.</returns>
        /// 
        public static Complex Multiply(Complex a, double s)
        {
            return new Complex(a.Re * s, a.Im * s);
        }

        /// <summary>
        /// Multiplies two complex numbers and puts the result in a third complex number.
        /// </summary>
        /// 
        /// <param name="a">A <see cref="Complex"/> instance.</param>
        /// <param name="b">A <see cref="Complex"/> instance.</param>
        /// <param name="result">A <see cref="Complex"/> instance to hold the result.</param>
        /// 
        public static void Multiply(Complex a, Complex b, ref Complex result)
        {
            // (x + yi)(u + vi) = (xu  yv) + (xv + yu)i. 
            double aRe = a.Re, aIm = a.Im;
            double bRe = b.Re, bIm = b.Im;

            result.Re = aRe * bRe - aIm * bIm;
            result.Im = aRe * bIm + aIm * bRe;
        }

        /// <summary>
        /// Multiplies a complex number by a scalar value and puts the result into another complex number.
        /// </summary>
        /// 
        /// <param name="a">A <see cref="Complex"/> instance.</param>
        /// <param name="s">A scalar value.</param>
        /// <param name="result">A <see cref="Complex"/> instance to hold the result.</param>
        /// 
        public static void Multiply(Complex a, double s, ref Complex result)
        {
            result.Re = a.Re * s;
            result.Im = a.Im * s;
        }

        /// <summary>
        /// Divides one complex number by another complex number.
        /// </summary>
        /// 
        /// <param name="a">A <see cref="Complex"/> instance.</param>
        /// <param name="b">A <see cref="Complex"/> instance.</param>
        /// 
        /// <returns>Returns new <see cref="Complex"/> instance containing the result.</returns>
        /// 
        /// <exception cref="DivideByZeroException">Can not divide by zero.</exception>
        /// 
        public static Complex Divide(Complex a, Complex b)
        {
            double aRe = a.Re, aIm = a.Im;
            double bRe = b.Re, bIm = b.Im;
            double modulusSquared = bRe * bRe + bIm * bIm;

            if (modulusSquared == 0)
            {
                throw new DivideByZeroException("Can not divide by zero.");
            }

            double invModulusSquared = 1 / modulusSquared;

            return new Complex(
                (aRe * bRe + aIm * bIm) * invModulusSquared,
                (aIm * bRe - aRe * bIm) * invModulusSquared);
        }

        /// <summary>
        /// Divides a complex number by a scalar value.
        /// </summary>
        /// 
        /// <param name="a">A <see cref="Complex"/> instance.</param>
        /// <param name="s">A scalar value.</param>
        /// 
        /// <returns>Returns new <see cref="Complex"/> instance containing the result.</returns>
        /// 
        /// <exception cref="DivideByZeroException">Can not divide by zero.</exception>
        /// 
        public static Complex Divide(Complex a, double s)
        {
            if (s == 0)
            {
                throw new DivideByZeroException("Can not divide by zero.");
            }

            return new Complex(a.Re / s, a.Im / s);
        }

        /// <summary>
        /// Divides a scalar value by a complex number.
        /// </summary>
        /// 
        /// <param name="s">A scalar value.</param>
        /// <param name="a">A <see cref="Complex"/> instance.</param>
        /// 
        /// <returns>Returns new <see cref="Complex"/> instance containing the result.</returns>
        /// 
        /// <exception cref="DivideByZeroException">Can not divide by zero.</exception>
        /// 
        public static Complex Divide(double s, Complex a)
        {
            if ((a.Re == 0) || (a.Im == 0))
            {
                throw new DivideByZeroException("Can not divide by zero.");
            }
            return new Complex(s / a.Re, s / a.Im);
        }

        /// <summary>
        /// Divides one complex number by another complex number and puts the result in a third complex number.
        /// </summary>
        /// 
        /// <param name="a">A <see cref="Complex"/> instance.</param>
        /// <param name="b">A <see cref="Complex"/> instance.</param>
        /// <param name="result">A <see cref="Complex"/> instance to hold the result.</param>
        /// 
        /// <exception cref="DivideByZeroException">Can not divide by zero.</exception>
        /// 
        public static void Divide(Complex a, Complex b, ref Complex result)
        {
            double aRe = a.Re, aIm = a.Im;
            double bRe = b.Re, bIm = b.Im;
            double modulusSquared = bRe * bRe + bIm * bIm;

            if (modulusSquared == 0)
            {
                throw new DivideByZeroException("Can not divide by zero.");
            }

            double invModulusSquared = 1 / modulusSquared;

            result.Re = (aRe * bRe + aIm * bIm) * invModulusSquared;
            result.Im = (aIm * bRe - aRe * bIm) * invModulusSquared;
        }

        /// <summary>
        /// Divides a complex number by a scalar value and puts the result into another complex number.
        /// </summary>
        /// 
        /// <param name="a">A <see cref="Complex"/> instance.</param>
        /// <param name="s">A scalar value.</param>
        /// <param name="result">A <see cref="Complex"/> instance to hold the result.</param>
        /// 
        /// <exception cref="DivideByZeroException">Can not divide by zero.</exception>
        /// 
        public static void Divide(Complex a, double s, ref Complex result)
        {
            if (s == 0)
            {
                throw new DivideByZeroException("Can not divide by zero.");
            }

            result.Re = a.Re / s;
            result.Im = a.Im / s;
        }

        /// <summary>
        /// Divides a scalar value by a complex number and puts the result into another complex number.
        /// </summary>
        /// 
        /// <param name="a">A <see cref="Complex"/> instance.</param>
        /// <param name="s">A scalar value.</param>
        /// <param name="result">A <see cref="Complex"/> instance to hold the result.</param>
        /// 
        /// <exception cref="DivideByZeroException">Can not divide by zero.</exception>
        /// 
        public static void Divide(double s, Complex a, ref Complex result)
        {
            if ((a.Re == 0) || (a.Im == 0))
            {
                throw new DivideByZeroException("Can not divide by zero.");
            }

            result.Re = s / a.Re;
            result.Im = s / a.Im;
        }

        /// <summary>
        /// Negates a complex number.
        /// </summary>
        /// 
        /// <param name="a">A <see cref="Complex"/> instance.</param>
        /// 
        /// <returns>Returns new <see cref="Complex"/> instance containing the negated values.</returns>
        /// 
        public static Complex Negate(Complex a)
        {
            return new Complex(-a.Re, -a.Im);
        }

        /// <summary>
        /// Tests whether two complex numbers are approximately equal using default tolerance value.
        /// </summary>
        /// 
        /// <param name="a">A <see cref="Complex"/> instance.</param>
        /// <param name="b">A <see cref="Complex"/> instance.</param>
        /// 
        /// <returns>Return <see langword="true"/> if the two vectors are approximately equal or <see langword="false"/> otherwise.</returns>
        /// 
        /// <remarks><para>The default tolerance value, which is used for the test, equals to 8.8817841970012523233891E-16.</para></remarks>
        /// 
        public static bool ApproxEqual(Complex a, Complex b)
        {
            return ApproxEqual(a, b, 8.8817841970012523233891E-16);
        }


        /// <summary>
        /// Tests whether two complex numbers are approximately equal given a tolerance value.
        /// </summary>
        /// 
        /// <param name="a">A <see cref="Complex"/> instance.</param>
        /// <param name="b">A <see cref="Complex"/> instance.</param>
        /// <param name="tolerance">The tolerance value used to test approximate equality.</param>
        /// 
        /// <remarks><para>The default tolerance value, which is used for the test, equals to 8.8817841970012523233891E-16.</para></remarks>
        /// 
        public static bool ApproxEqual(Complex a, Complex b, double tolerance)
        {
            return
                (
                (System.Math.Abs(a.Re - b.Re) <= tolerance) &&
                (System.Math.Abs(a.Im - b.Im) <= tolerance)
                );
        }

        #region Public Static Parse Methods
        /// <summary>
        /// Converts the specified string to its <see cref="Complex"/> equivalent.
        /// </summary>
        /// 
        /// <param name="s">A string representation of a complex number.</param>
        /// 
        /// <returns>Returns new <see cref="Complex"/> instance that represents the complex number
        /// specified by the <paramref name="s"/> parameter.</returns>
        /// 
        /// <exception cref="FormatException">String representation of the complex number is not correctly formatted.</exception>
        /// 
        public static Complex Parse(string s)
        {
            Regex r = new Regex(@"\((?<real>.*),(?<imaginary>.*)\)", RegexOptions.None);
            Match m = r.Match(s);

            if (m.Success)
            {
                return new Complex(
                    double.Parse(m.Result("${real}")),
                    double.Parse(m.Result("${imaginary}"))
                    );
            }
            else
            {
                throw new FormatException("String representation of the complex number is not correctly formatted.");
            }
        }

        /// <summary>
        /// Try to convert the specified string to its <see cref="Complex"/> equivalent.
        /// </summary>
        /// 
        /// <param name="s">A string representation of a complex number.</param>
        /// 
        /// <param name="result"><see cref="Complex"/> instance to output the result to.</param>
        /// 
        /// <returns>Returns boolean value that indicates if the parse was successful or not.</returns>
        /// 
        public static bool TryParse(string s, out Complex result)
        {
            try
            {
                Complex newComplex = Complex.Parse(s);
                result = newComplex;
                return true;

            }
            catch (FormatException)
            {

                result = new Complex();
                return false;
            }
        }

        #endregion

        #region Public Static Complex Special Functions

        /// <summary>
        /// Calculates square root of a complex number.
        /// </summary>
        /// 
        /// <param name="a">A <see cref="Complex"/> instance.</param>
        /// 
        /// <returns>Returns new <see cref="Complex"/> instance containing the square root of the specified
        /// complex number.</returns>
        /// 
        public static Complex Sqrt(Complex a)
        {
            Complex result = Complex.Zero;

            if ((a.Re == 0.0) && (a.Im == 0.0))
            {
                return result;
            }
            else if (a.Im == 0.0)
            {
                result.Re = (a.Re > 0) ? System.Math.Sqrt(a.Re) : System.Math.Sqrt(-a.Re);
                result.Im = 0.0;
            }
            else
            {
                double modulus = a.Magnitude;

                result.Re = System.Math.Sqrt(0.5 * (modulus + a.Re));
                result.Im = System.Math.Sqrt(0.5 * (modulus - a.Re));
                if (a.Im < 0.0)
                    result.Im = -result.Im;
            }

            return result;
        }

        /// <summary>
        /// Calculates natural (base <b>e</b>) logarithm of a complex number.
        /// </summary>
        /// 
        /// <param name="a">A <see cref="Complex"/> instance.</param>
        /// 
        /// <returns>Returns new <see cref="Complex"/> instance containing the natural logarithm of the specified
        /// complex number.</returns>
        /// 
        public static Complex Log(Complex a)
        {
            Complex result = Complex.Zero;

            if ((a.Re > 0.0) && (a.Im == 0.0))
            {
                result.Re = System.Math.Log(a.Re);
                result.Im = 0.0;
            }
            else if (a.Re == 0.0)
            {
                if (a.Im > 0.0)
                {
                    result.Re = System.Math.Log(a.Im);
                    result.Im = System.Math.PI / 2.0;
                }
                else
                {
                    result.Re = System.Math.Log(-(a.Im));
                    result.Im = -System.Math.PI / 2.0;
                }
            }
            else
            {
                result.Re = System.Math.Log(a.Magnitude);
                result.Im = System.Math.Atan2(a.Im, a.Re);
            }

            return result;
        }

        /// <summary>
        /// Calculates exponent (<b>e</b> raised to the specified power) of a complex number.
        /// </summary>
        /// 
        /// <param name="a">A <see cref="Complex"/> instance.</param>
        /// 
        /// <returns>Returns new <see cref="Complex"/> instance containing the exponent of the specified
        /// complex number.</returns>
        /// 
        public static Complex Exp(Complex a)
        {
            Complex result = Complex.Zero;
            double r = System.Math.Exp(a.Re);
            result.Re = r * System.Math.Cos(a.Im);
            result.Im = r * System.Math.Sin(a.Im);

            return result;
        }
        #endregion

        #region Public Static Complex Trigonometry

        /// <summary>
        /// Calculates Sine value of the complex number.
        /// </summary>
        /// 
        /// <param name="a">A <see cref="Complex"/> instance.</param>
        /// 
        /// <returns>Returns new <see cref="Complex"/> instance containing the Sine value of the specified
        /// complex number.</returns>
        /// 
        public static Complex Sin(Complex a)
        {
            Complex result = Complex.Zero;

            if (a.Im == 0.0)
            {
                result.Re = System.Math.Sin(a.Re);
                result.Im = 0.0;
            }
            else
            {
                result.Re = System.Math.Sin(a.Re) * System.Math.Cosh(a.Im);
                result.Im = System.Math.Cos(a.Re) * System.Math.Sinh(a.Im);
            }

            return result;
        }

        /// <summary>
        /// Calculates Cosine value of the complex number.
        /// </summary>
        /// 
        /// <param name="a">A <see cref="Complex"/> instance.</param>
        /// 
        /// <returns>Returns new <see cref="Complex"/> instance containing the Cosine value of the specified
        /// complex number.</returns>
        /// 
        public static Complex Cos(Complex a)
        {
            Complex result = Complex.Zero;

            if (a.Im == 0.0)
            {
                result.Re = System.Math.Cos(a.Re);
                result.Im = 0.0;
            }
            else
            {
                result.Re = System.Math.Cos(a.Re) * System.Math.Cosh(a.Im);
                result.Im = -System.Math.Sin(a.Re) * System.Math.Sinh(a.Im);
            }

            return result;
        }

        /// <summary>
        /// Calculates Tangent value of the complex number.
        /// </summary>
        /// 
        /// <param name="a">A <see cref="Complex"/> instance.</param>
        /// 
        /// <returns>Returns new <see cref="Complex"/> instance containing the Tangent value of the specified
        /// complex number.</returns>
        /// 
        public static Complex Tan(Complex a)
        {
            Complex result = Complex.Zero;

            if (a.Im == 0.0)
            {
                result.Re = System.Math.Tan(a.Re);
                result.Im = 0.0;
            }
            else
            {
                double real2 = 2 * a.Re;
                double imag2 = 2 * a.Im;
                double denom = System.Math.Cos(real2) + System.Math.Cosh(real2);

                result.Re = System.Math.Sin(real2) / denom;
                result.Im = System.Math.Sinh(imag2) / denom;
            }

            return result;
        }
        #endregion

        #region Overrides
        /// <summary>
        /// Returns the hashcode for this instance.
        /// </summary>
        /// 
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
        {
            return Re.GetHashCode() ^ Im.GetHashCode();
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to the specified object.
        /// </summary>
        /// 
        /// <param name="obj">An object to compare to this instance.</param>
        /// 
        /// <returns>Returns <see langword="true"/> if <paramref name="obj"/> is a <see cref="Complex"/> and has the same values as this instance or <see langword="false"/> otherwise.</returns>
        /// 
        public override bool Equals(object obj)
        {
            return (obj is Complex) ? (this == (Complex)obj) : false;
        }

        /// <summary>
        /// Returns a string representation of this object.
        /// </summary>
        /// 
        /// <returns>A string representation of this object.</returns>
        /// 
        public override string ToString()
        {
            return string.Format("({0}, {1})", Re, Im);
        }
        #endregion

        #region Comparison Operators
        /// <summary>
        /// Tests whether two specified complex numbers are equal.
        /// </summary>
        /// 
        /// <param name="u">The left-hand complex number.</param>
        /// <param name="v">The right-hand complex number.</param>
        /// 
        /// <returns>Returns <see langword="true"/> if the two complex numbers are equal or <see langword="false"/> otherwise.</returns>
        /// 
        public static bool operator ==(Complex u, Complex v)
        {
            return ((u.Re == v.Re) && (u.Im == v.Im));
        }

        /// <summary>
        /// Tests whether two specified complex numbers are not equal.
        /// </summary>
        /// 
        /// <param name="u">The left-hand complex number.</param>
        /// <param name="v">The right-hand complex number.</param>
        /// 
        /// <returns>Returns <see langword="true"/> if the two complex numbers are not equal or <see langword="false"/> otherwise.</returns>
        /// 
        public static bool operator !=(Complex u, Complex v)
        {
            return !(u == v);
        }
        #endregion

        #region Unary Operators
        /// <summary>
        /// Negates the complex number.
        /// </summary>
        /// 
        /// <param name="a">A <see cref="Complex"/>  instance.</param>
        /// 
        /// <returns>Returns new <see cref="Complex"/> instance containing the negated values.</returns>
        /// 
        public static Complex operator -(Complex a)
        {
            return Complex.Negate(a);
        }
        #endregion

        #region Binary Operators
        /// <summary>
        /// Adds two complex numbers.
        /// </summary>
        /// 
        /// <param name="a">A <see cref="Complex"/> instance.</param>
        /// <param name="b">A <see cref="Complex"/> instance.</param>
        /// 
        /// <returns>Returns new <see cref="Complex"/> instance containing the sum.</returns>
        /// 
        public static Complex operator +(Complex a, Complex b)
        {
            return Complex.Add(a, b);
        }

        /// <summary>
        /// Adds a complex number and a scalar value.
        /// </summary>
        /// 
        /// <param name="a">A <see cref="Complex"/> instance.</param>
        /// <param name="s">A scalar value.</param>
        /// 
        /// <returns>Returns new <see cref="Complex"/> instance containing the sum.</returns>
        /// 
        public static Complex operator +(Complex a, double s)
        {
            return Complex.Add(a, s);
        }

        /// <summary>
        /// Adds a complex number and a scalar value.
        /// </summary>
        /// 
        /// <param name="a">A <see cref="Complex"/> instance.</param>
        /// <param name="s">A scalar value.</param>
        /// 
        /// <returns>Returns new <see cref="Complex"/> instance containing the sum.</returns>
        /// 
        public static Complex operator +(double s, Complex a)
        {
            return Complex.Add(a, s);
        }

        /// <summary>
        /// Subtracts one complex number from another complex number.
        /// </summary>
        /// 
        /// <param name="a">A <see cref="Complex"/> instance.</param>
        /// <param name="b">A <see cref="Complex"/> instance.</param>
        /// 
        /// <returns>Returns new <see cref="Complex"/> instance containing the difference.</returns>
        /// 
        public static Complex operator -(Complex a, Complex b)
        {
            return Complex.Subtract(a, b);
        }

        /// <summary>
        /// Subtracts a scalar value from a complex number.
        /// </summary>
        /// 
        /// <param name="a">A <see cref="Complex"/> instance.</param>
        /// <param name="s">A scalar value.</param>
        /// 
        /// <returns>Returns new <see cref="Complex"/> instance containing the difference.</returns>
        /// 
        public static Complex operator -(Complex a, double s)
        {
            return Complex.Subtract(a, s);
        }

        /// <summary>
        /// Subtracts a complex number from a scalar value.
        /// </summary>
        /// 
        /// <param name="s">A scalar value.</param>
        /// <param name="a">A <see cref="Complex"/> instance.</param>
        /// 
        /// <returns>Returns new <see cref="Complex"/> instance containing the difference.</returns>
        /// 
        public static Complex operator -(double s, Complex a)
        {
            return Complex.Subtract(s, a);
        }

        /// <summary>
        /// Multiplies two complex numbers.
        /// </summary>
        /// 
        /// <param name="a">A <see cref="Complex"/> instance.</param>
        /// <param name="b">A <see cref="Complex"/> instance.</param>
        /// 
        /// <returns>Returns new <see cref="Complex"/> instance containing the result of multiplication.</returns>
        /// 
        public static Complex operator *(Complex a, Complex b)
        {
            return Complex.Multiply(a, b);
        }

        /// <summary>
        /// Multiplies a complex number by a scalar value.
        /// </summary>
        /// 
        /// <param name="s">A scalar value.</param>
        /// <param name="a">A <see cref="Complex"/> instance.</param>
        /// 
        /// <returns>Returns new <see cref="Complex"/> instance containing the result of multiplication.</returns>
        /// 
        public static Complex operator *(double s, Complex a)
        {
            return Complex.Multiply(a, s);
        }

        /// <summary>
        /// Multiplies a complex number by a scalar value.
        /// </summary>
        /// 
        /// <param name="a">A <see cref="Complex"/> instance.</param>
        /// <param name="s">A scalar value.</param>
        /// 
        /// <returns>Returns new <see cref="Complex"/> instance containing the result of multiplication.</returns>
        /// 
        public static Complex operator *(Complex a, double s)
        {
            return Complex.Multiply(a, s);
        }

        /// <summary>
        /// Divides one complex number by another complex number.
        /// </summary>
        /// 
        /// <param name="a">A <see cref="Complex"/> instance.</param>
        /// <param name="b">A <see cref="Complex"/> instance.</param>
        /// 
        /// <returns>A new Complex instance containing the result.</returns>
        /// <returns>Returns new <see cref="Complex"/> instance containing the result of division.</returns>
        /// 
        public static Complex operator /(Complex a, Complex b)
        {
            return Complex.Divide(a, b);
        }

        /// <summary>
        /// Divides a complex number by a scalar value.
        /// </summary>
        /// 
        /// <param name="a">A <see cref="Complex"/> instance.</param>
        /// <param name="s">A scalar value.</param>
        /// 
        /// <returns>Returns new <see cref="Complex"/> instance containing the result of division.</returns>
        /// 
        public static Complex operator /(Complex a, double s)
        {
            return Complex.Divide(a, s);
        }

        /// <summary>
        /// Divides a scalar value by a complex number.
        /// </summary>
        /// 
        /// <param name="a">A <see cref="Complex"/> instance.</param>
        /// <param name="s">A scalar value.</param>
        /// 
        /// <returns>Returns new <see cref="Complex"/> instance containing the result of division.</returns>
        /// 
        public static Complex operator /(double s, Complex a)
        {
            return Complex.Divide(s, a);
        }
        #endregion

        #region Conversion Operators
        /// <summary>
        /// Converts from a single-precision real number to a complex number. 
        /// </summary>
        /// 
        /// <param name="value">Single-precision real number to convert to complex number.</param>
        /// 
        /// <returns>Returns new <see cref="Complex"/> instance containing complex number with
        /// real part initialized to the specified value.</returns>
        /// 
        public static explicit operator Complex(float value)
        {
            return new Complex((double)value, 0);
        }

        /// <summary>
        /// Converts from a double-precision real number to a complex number. 
        /// </summary>
        /// 
        /// <param name="value">Double-precision real number to convert to complex number.</param>
        /// 
        /// <returns>Returns new <see cref="Complex"/> instance containing complex number with
        /// real part initialized to the specified value.</returns>
        /// 
        public static explicit operator Complex(double value)
        {
            return new Complex(value, 0);
        }
        #endregion

        #region ICloneable Members
        /// <summary>
        /// Creates an exact copy of this <see cref="Complex"/> object.
        /// </summary>
        /// 
        /// <returns>Returns clone of the complex number.</returns>
        /// 
        object ICloneable.Clone()
        {
            return new Complex(this);
        }

        /// <summary>
        /// Creates an exact copy of this <see cref="Complex"/> object.
        /// </summary>
        /// 
        /// <returns>Returns clone of the complex number.</returns>
        /// 
        public Complex Clone()
        {
            return new Complex(this);
        }
        #endregion

        #region ISerializable Members
        /// <summary>
        /// Populates a <see cref="SerializationInfo"/> with the data needed to serialize the target object.
        /// </summary>
        /// 
        /// <param name="info">The <see cref="SerializationInfo"/> to populate with data. </param>
        /// <param name="context">The destination (see <see cref="StreamingContext"/>) for this serialization.</param>
        /// 
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Real", this.Re);
            info.AddValue("Imaginary", this.Im);
        }
        #endregion
    }
}
// AForge Math Library
// AForge.NET framework
// http://www.aforgenet.com/framework/
//
// Copyright © Andrew Kirillov, 2005-2009
// andrew.kirillov@aforgenet.com
//

namespace AForge.Math
{
    using System;

    /// <summary>
    /// Set of tool functions.
    /// </summary>
    /// 
    /// <remarks>The class contains different utility functions.</remarks>
    /// 
    public static class Tools
    {
        /// <summary>
        /// Calculates power of 2.
        /// </summary>
        /// 
        /// <param name="power">Power to raise in.</param>
        /// 
        /// <returns>Returns specified power of 2 in the case if power is in the range of
        /// [0, 30]. Otherwise returns 0.</returns>
        /// 
        public static int Pow2(int power)
        {
            return ((power >= 0) && (power <= 30)) ? (1 << power) : 0;
        }

        /// <summary>
        /// Checks if the specified integer is power of 2.
        /// </summary>
        /// 
        /// <param name="x">Integer number to check.</param>
        /// 
        /// <returns>Returns <b>true</b> if the specified number is power of 2.
        /// Otherwise returns <b>false</b>.</returns>
        /// 
        public static bool IsPowerOf2(int x)
        {
            return (x > 0) ? ((x & (x - 1)) == 0) : false;
        }

        /// <summary>
        /// Get base of binary logarithm.
        /// </summary>
        /// 
        /// <param name="x">Source integer number.</param>
        /// 
        /// <returns>Power of the number (base of binary logarithm).</returns>
        /// 
        public static int Log2(int x)
        {
            if (x <= 65536)
            {
                if (x <= 256)
                {
                    if (x <= 16)
                    {
                        if (x <= 4)
                        {
                            if (x <= 2)
                            {
                                if (x <= 1)
                                    return 0;
                                return 1;
                            }
                            return 2;
                        }
                        if (x <= 8)
                            return 3;
                        return 4;
                    }
                    if (x <= 64)
                    {
                        if (x <= 32)
                            return 5;
                        return 6;
                    }
                    if (x <= 128)
                        return 7;
                    return 8;
                }
                if (x <= 4096)
                {
                    if (x <= 1024)
                    {
                        if (x <= 512)
                            return 9;
                        return 10;
                    }
                    if (x <= 2048)
                        return 11;
                    return 12;
                }
                if (x <= 16384)
                {
                    if (x <= 8192)
                        return 13;
                    return 14;
                }
                if (x <= 32768)
                    return 15;
                return 16;
            }

            if (x <= 16777216)
            {
                if (x <= 1048576)
                {
                    if (x <= 262144)
                    {
                        if (x <= 131072)
                            return 17;
                        return 18;
                    }
                    if (x <= 524288)
                        return 19;
                    return 20;
                }
                if (x <= 4194304)
                {
                    if (x <= 2097152)
                        return 21;
                    return 22;
                }
                if (x <= 8388608)
                    return 23;
                return 24;
            }
            if (x <= 268435456)
            {
                if (x <= 67108864)
                {
                    if (x <= 33554432)
                        return 25;
                    return 26;
                }
                if (x <= 134217728)
                    return 27;
                return 28;
            }
            if (x <= 1073741824)
            {
                if (x <= 536870912)
                    return 29;
                return 30;
            }
            return 31;
        }
    }
}

