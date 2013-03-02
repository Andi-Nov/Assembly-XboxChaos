﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExtryzeDLL.Patching
{
    /// <summary>
    /// Represents changes made to a segment in a cache file.
    /// </summary>
    public class SegmentChange
    {
        public SegmentChange(uint oldOffset, int oldActualSize, uint newOffset, int newActualSize, bool resizeAtEnd)
        {
            OldOffset = oldOffset;
            OldSize = oldActualSize;
            NewOffset = newOffset;
            NewSize = newActualSize;
            ResizeAtEnd = resizeAtEnd;
            
            DataChanges = new List<DataChange>();
        }

        /// <summary>
        /// The offset of the segment in the unmodified file.
        /// </summary>
        public uint OldOffset { get; set; }

        /// <summary>
        /// The old size of the segment including padding.
        /// </summary>
        public int OldSize { get; set; }

        /// <summary>
        /// The offset of the segment in the file after all segments have been resized.
        /// </summary>
        public uint NewOffset { get; set; }

        /// <summary>
        /// The new size of the segment including padding.
        /// </summary>
        public int NewSize { get; set; }

        /// <summary>
        /// True if extra data should be added at the end of the segment when resized,
        /// false if it should be added at the beginning.
        /// </summary>
        public bool ResizeAtEnd { get; set; }

        /// <summary>
        /// The changes that should be made to the segment's data.
        /// </summary>
        public List<DataChange> DataChanges { get; set; }
    }
}
