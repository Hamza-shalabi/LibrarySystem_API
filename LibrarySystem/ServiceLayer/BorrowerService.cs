using LibrarySystem.DTOs.BorrowerDTO;
using LibrarySystem.Interfaces;
using LibrarySystem.Mapper;
using LibrarySystem.Models;

namespace LibrarySystem.ServiceLayer
{
    public class BorrowerService(IGenericRepository<Borrower> repo)
    {
        private readonly IGenericRepository<Borrower> _repo = repo;

        public async Task<List<BorrowerDto>> GetBorrowers()
        {
            var borrowers = await _repo.GetAllAsync();
            var borrowersDto = borrowers.Select(b => b.ToBorrowerDto()).ToList();
            return borrowersDto;
        }
        public async Task<BorrowerDto?> GetBorrower(int id)
        {
            var isExists = await _repo.IsExistAsync(id);
            if (!isExists)
                return null;

            var borrower = await _repo.GetAsync(id);
            return borrower?.ToBorrowerDto();
        }
        public async Task<BorrowerDto> CreateBorrower(Borrower borrower)
        {
            await _repo.CreateAsync(borrower);
            return borrower.ToBorrowerDto();
        }
        public async Task<bool> DeleteBorrower(int id)
        {
            var isExists = await _repo.IsExistAsync(id);
            if (!isExists)
                return false;
            await _repo.DeleteAsync(id);
            return true;
        }

        public async Task<BorrowerDto?> UpdateBorrower(int id, UpdateBorrowerRequestDto borrowerDto)
        {
            var isExists = await _repo.IsExistAsync(id);
            if (!isExists)
                return null;

            var originalBorrower = await _repo.GetAsync(id);
            if (originalBorrower == null)
                return null;

            originalBorrower.Name = borrowerDto.Name;
            originalBorrower.Email = borrowerDto.Email;
            originalBorrower.Phone = borrowerDto.Phone;
            originalBorrower.LastUpdatedAt = DateTime.Now.ToUniversalTime();

            var updatedBorrower = await _repo.UpdateAsync(originalBorrower);
            return updatedBorrower?.ToBorrowerDto();
        }
    }
}
