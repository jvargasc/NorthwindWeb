﻿using System;
using System.IO;

namespace Northwind.API
{
	public static class Utilities
	{
		internal static string Base64String(byte[] Picture)
		{
			var base64Str = string.Empty;
			using (var ms = new MemoryStream())
			{
				int offset = 78;
				ms.Write(Picture, offset, Picture.Length - offset);
				var bmp = new System.Drawing.Bitmap(ms);
				using (var jpegms = new MemoryStream())
				{
					bmp.Save(jpegms, System.Drawing.Imaging.ImageFormat.Jpeg);
					base64Str = Convert.ToBase64String(jpegms.ToArray());
				}
			}
			return base64Str;
		}

		internal static void getPageSizes(int recordCount, int itemsPerPage, int page, out int pageToTake, out int pageToSkip)
		{
			pageToTake = 0;
			pageToSkip = 0;

			if (recordCount <= itemsPerPage)
			{
				pageToSkip = 0;
				pageToTake = recordCount;
			}
			else
			{
				pageToSkip = ((page - 1)* itemsPerPage);
				pageToTake = itemsPerPage;
			}		
		}
	}
}
