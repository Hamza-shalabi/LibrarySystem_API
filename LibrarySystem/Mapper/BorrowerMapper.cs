using LibrarySystem.DTOs.BorrowerDTO;
using LibrarySystem.Models;

namespace LibrarySystem.Mapper
{
    public static class BorrowerMapper
    {
        public static BorrowerDto ToBorrowerDto(this Borrower borrower)
        {
            return new BorrowerDto
            {
                Id = borrower.Id,
                Name = borrower.Name,
                Email = borrower.Email,
                Phone = borrower.Phone,
            };
        }

        public static BorrowerIncludeDto ToBorrowerIncludeDto(this Borrower borrower)
        {
            return new BorrowerIncludeDto
            {
                Id = borrower.Id,
                Name = borrower.Name,
                Email = borrower.Email,
                Phone = borrower.Phone
            };
        }

        public static Borrower ToBorrowerFromCreateDto(this CreateBorrowerRequestDto borrower)
        {
            return new Borrower
            {
                Name = borrower.Name,
                Email = borrower.Email,
                Phone = borrower.Phone,
            };
        }

        public static Borrower ToBorrowerFromUpdateDto(this UpdateBorrowerRequestDto borrower)
        {
            return new Borrower
            {
                Id = borrower.Id,
                Name = borrower.Name,
                Email = borrower.Email,
                Phone = borrower.Phone,
                LastUpdatedAt = DateTime.Now.ToUniversalTime(),
            };
        }
    }
}
