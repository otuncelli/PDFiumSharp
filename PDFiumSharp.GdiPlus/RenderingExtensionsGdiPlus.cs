﻿#region Copyright and License
/*
This file is part of PDFiumSharp, a wrapper around the PDFium library for the .NET framework.
Copyright (C) 2017 Tobias Meyer
License: Microsoft Reciprocal License (MS-RL)
*/
#endregion
using System;
using System.Drawing;
using PDFiumSharp.Enums;

namespace PDFiumSharp
{
	public static class RenderingExtensionsGdiPlus
	{
		/// <summary>
		/// Renders the page to a <see cref="Bitmap"/>
		/// </summary>
		/// <param name="page">The page which is to be rendered.</param>
		/// <param name="renderTarget">The bitmap to which the page is to be rendered.</param>
		/// <param name="rectDest">The destination rectangle in <paramref name="renderTarget"/>.</param>
		/// <param name="orientation">The orientation at which the page is to be rendered.</param>
		/// <param name="flags">The flags specifying how the page is to be rendered.</param>
		public static void Render(this PdfPage page, Bitmap renderTarget, Rectangle rectDest, PageOrientations orientation = PageOrientations.Normal, RenderingFlags flags = RenderingFlags.None)
		{
			if (renderTarget == null)
				throw new ArgumentNullException(nameof(renderTarget));

			var format = GetBitmapFormat(renderTarget);
			var data = renderTarget.LockBits(new System.Drawing.Rectangle(0, 0, renderTarget.Width, renderTarget.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, renderTarget.PixelFormat);
			using (var tmp = new PDFiumBitmap(renderTarget.Width, renderTarget.Height, format, data.Scan0, data.Stride))
				page.Render(tmp, rectDest, orientation, flags);
			renderTarget.UnlockBits(data);
		}

		/// <summary>
		/// Renders the page to a <see cref="Bitmap"/>
		/// </summary>
		/// <param name="page">The page which is to be rendered.</param>
		/// <param name="bitmap">The bitmap to which the page is to be rendered.</param>
		/// <param name="orientation">The orientation at which the page is to be rendered.</param>
		/// <param name="flags">The flags specifying how the page is to be rendered.</param>
		public static void Render(this PdfPage page, Bitmap bitmap, PageOrientations orientation = PageOrientations.Normal, RenderingFlags flags = RenderingFlags.None)
		{
			page.Render(bitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height), orientation, flags);
		}

		static BitmapFormats GetBitmapFormat(Bitmap bitmap)
		{
			switch (bitmap.PixelFormat) 
            {
                case System.Drawing.Imaging.PixelFormat.Format24bppRgb:
                    return BitmapFormats.BGR;
                case System.Drawing.Imaging.PixelFormat.Format32bppArgb:
                    return BitmapFormats.BGRA;
                case System.Drawing.Imaging.PixelFormat.Format32bppRgb:
                    return BitmapFormats.BGRx;
                default:
                    throw new NotSupportedException($"Pixel format {bitmap.PixelFormat} is not supported.");
            }
        }
	}
}
