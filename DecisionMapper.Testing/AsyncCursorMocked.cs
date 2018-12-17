using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace DecisionMapper.Testing.Infrastructure
{
    public class AsyncCursorMocked<TProjection> : IAsyncCursor<TProjection>
    {
        private List<List<TProjection>> batches = new List<List<TProjection>>();
        private int batchIndex = -1;

        public AsyncCursorMocked(IEnumerable<object> items)
        {
            var currentBatch = items.Select(x => (TProjection)x).ToList();
            batches.Add(currentBatch);
        }

        public IEnumerable<TProjection> Current => batches[batchIndex];

        public void Dispose()
        {
            // Nothing to do here
        }

        public bool MoveNext(CancellationToken cancellationToken = default(CancellationToken))
        {
            batchIndex++;
            return batchIndex < batches.Count;
        }

        public Task<bool> MoveNextAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            batchIndex++;
            return Task.FromResult(batchIndex < batches.Count);
        }
    }
}
