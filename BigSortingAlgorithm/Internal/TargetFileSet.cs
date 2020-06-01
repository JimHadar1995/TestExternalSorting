using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace BigSortingAlgorithm.Internal
{
    internal sealed class TargetFileSet : IEnumerable<TargetFile>
    {
        private readonly List<TargetFile> _files;

        internal TargetFileSet(int capacity)
        {
            _files = new List<TargetFile>(capacity);
        }

        internal TargetFileSet()
        {
            _files = new List<TargetFile>();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<TargetFile> GetEnumerator()
        {
            return ((IEnumerable<TargetFile>)_files).GetEnumerator();
        }


        public void Add(TargetFile file)
        {
            _files.Add(file);
        }
    }
}
