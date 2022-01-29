#region Copyright and License
/*
This file is part of PDFiumSharp, a wrapper around the PDFium library for the .NET framework.
Copyright (C) 2017 Tobias Meyer
License: Microsoft Reciprocal License (MS-RL)
*/
#endregion
using System;
using System.Collections.Generic;
using System.Drawing;
using PDFiumSharp.Enums;
using PDFiumSharp.Types;

namespace PDFiumSharp
{
    public sealed class PdfPage : NativeWrapper<FPDF_PAGE>
    {
		/// <summary>
		/// Gets the page width (excluding non-displayable area) measured in points.
		/// One point is 1/72 inch(around 0.3528 mm).
		/// </summary>
		public float Width => PDFium.FPDF_GetPageWidthF(Handle);

		/// <summary>
		/// Gets the page height (excluding non-displayable area) measured in points.
		/// One point is 1/72 inch(around 0.3528 mm).
		/// </summary>
		public float Height => PDFium.FPDF_GetPageHeightF(Handle);

		/// <summary>
		/// Gets the page width and height (excluding non-displayable area) measured in points.
		/// One point is 1/72 inch(around 0.3528 mm).
		/// </summary>
		public SizeF Size
		{
			get
			{
				if (PDFium.FPDF_GetPageSizeByIndexF(Document.Handle, Index, out FS_SIZEF size))
				{
					return new SizeF(size.Width, size.Height);
				}
				throw new PDFiumException();
			}
		}

		/// <summary>
		/// Gets the page orientation.
		/// </summary>
		public PageOrientations Orientation
		{
			get => PDFium.FPDFPage_GetRotation(Handle);
			set => PDFium.FPDFPage_SetRotation(Handle, value);
		}

        /// <summary>
        /// Get the transparency of the page
        /// </summary>
        public bool HasTransparency => PDFium.FPDFPage_HasTransparency(Handle);

        /// <summary>
        /// Gets the zero-based index of the page in the <see cref="Document"/>
        /// </summary>
        public int Index { get; internal set; } = -1;

		/// <summary>
		/// Gets the <see cref="PdfDocument"/> which contains the page.
		/// </summary>
		public PdfDocument Document { get; }

		//public string Label => PDFium.FPDF_GetPageLabel(Document.Handle, Index);

		PdfPage(PdfDocument doc, FPDF_PAGE page, int index)
			: base(page)
		{
			if (page.IsNull)
			{
				throw new PDFiumException();
			}
			Document = doc;
			Index = index;
		}

		internal static PdfPage Load(PdfDocument doc, int index)
		{
			return new PdfPage(doc, PDFium.FPDF_LoadPage(doc.Handle, index), index);
		}
		internal static PdfPage New(PdfDocument doc, int index, double width, double height)
		{
			return new PdfPage(doc, PDFium.FPDFPage_New(doc.Handle, index, width, height), index);
		}

		/// <summary>
		/// Renders the page to a <see cref="PDFiumBitmap"/>
		/// </summary>
		/// <param name="renderTarget">The bitmap to which the page is to be rendered.</param>
		/// <param name="rectDest">The destination rectangle in <paramref name="renderTarget"/>.</param>
		/// <param name="orientation">The orientation at which the page is to be rendered.</param>
		/// <param name="flags">The flags specifying how the page is to be rendered.</param>
		public void Render(PDFiumBitmap renderTarget, Rectangle rectDest, PageOrientations orientation = PageOrientations.Normal, RenderingFlags flags = RenderingFlags.None)
		{
			if (renderTarget == null)
			{
				throw new ArgumentNullException(nameof(renderTarget));
			}

			PDFium.FPDF_RenderPageBitmap(renderTarget.Handle, Handle, rectDest.Left, rectDest.Top, rectDest.Width, rectDest.Height, orientation, flags);
		}

		/// <summary>
		/// Renders the page to a <see cref="PDFiumBitmap"/>
		/// </summary>
		/// <param name="renderTarget">The bitmap to which the page is to be rendered.</param>
		/// <param name="orientation">The orientation at which the page is to be rendered.</param>
		/// <param name="flags">The flags specifying how the page is to be rendered.</param>
		public void Render(PDFiumBitmap renderTarget, PageOrientations orientation = PageOrientations.Normal, RenderingFlags flags = RenderingFlags.None)
		{
			Render(renderTarget, new Rectangle(0, 0, renderTarget.Width, renderTarget.Height), orientation, flags);
		}

		public SizeF DeviceToPage(Rectangle displayArea, int deviceX, int deviceY, PageOrientations orientation = PageOrientations.Normal)
		{
			PDFium.FPDF_DeviceToPage(Handle, displayArea.Left, displayArea.Top, displayArea.Width, displayArea.Height, orientation, deviceX, deviceY, out double x, out double y);
			return new SizeF((float)x, (float)y);
		}

		public Size PageToDevice(Rectangle displayArea, double pageX, double pageY, PageOrientations orientation = PageOrientations.Normal)
		{
			PDFium.FPDF_PageToDevice(Handle, displayArea.Left, displayArea.Top, displayArea.Width, displayArea.Height, orientation, pageX, pageY, out int x, out int y);
			return new Size(x, y);
		}

		public FlattenResults Flatten(FlattenFlags flags)
		{
			return PDFium.FPDFPage_Flatten(Handle, flags);
		}

		public void Dispose()
		{
			((IDisposable)this).Dispose();
		}

        protected override void Dispose(FPDF_PAGE handle)
		{
			PDFium.FPDF_ClosePage(handle);
		}
	}
}
