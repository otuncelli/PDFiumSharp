﻿#region Copyright and License
/*
This file is part of PDFiumSharp, a wrapper around the PDFium library for the .NET framework.
Copyright (C) 2017 Tobias Meyer
License: Microsoft Reciprocal License (MS-RL)
*/
#endregion
namespace PDFiumSharp.Enums
{
	public enum RenderingStatus : int
	{
		Reader = 0,

		ToBeContinued = 1,

		Done = 2,

		Failed = 3
	}
}
