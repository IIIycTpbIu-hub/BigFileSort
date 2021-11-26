using System.Threading.Tasks;

namespace BigFileSort.Sort
{
    public interface IFileSorter
    {
        void Sort(string fileName);

        Task SortAsync(string fileName);
    }
}