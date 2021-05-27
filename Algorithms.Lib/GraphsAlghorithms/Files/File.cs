using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms.Lib.GraphsAlghorithms.Files
{
    public class File
    {
        private readonly Stack<Batch> _batches;

        public int BatchLength { get; private set; }
        public int BatchCount => _batches.Count;
        public IEnumerable<Batch> Batches => _batches;
        public bool TryPopBatch(out Batch batch) => _batches.TryPop(out batch);
        public void Append(Batch batch)
        {
            if (_batches.Count == 0)
            {
                _batches.Push(batch);
                BatchLength = batch.Bytes.Length;
            }
            if (batch.Bytes.Length != BatchLength) throw new ArgumentException("Серии в файле должны быть одинаковой длины");
            _batches.Push(batch);
        }
    }

    public struct Batch
    {
        public ReadOnlyMemory<byte> Bytes { get; }
    }
}
