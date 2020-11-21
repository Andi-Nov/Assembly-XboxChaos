﻿using Blamite.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blamite.Blam.ThirdGen
{
	public class ThirdGenPointerExpander : IPointerExpander
	{
		private int _magic;

		public ThirdGenPointerExpander(int magic)
		{
			_magic = magic;
		}

		public bool IsValid { get { return _magic != -1; } }

		public long Expand(uint pointer)
		{
			if (IsValid && pointer != 0)
				return ((long)pointer << 2) + _magic;
			else
				return pointer;
		}

		public uint Contract(long pointer)
		{
			if (IsValid && pointer != 0)
				return (uint)((pointer - _magic) >> 2);
			else
				return (uint)pointer;
		}
	}
}
