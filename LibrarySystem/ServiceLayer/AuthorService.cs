using LibrarySystem.DTOs.AuthorDTO;
using LibrarySystem.Interfaces;
using LibrarySystem.Mapper;
using LibrarySystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Facade
{
    public class AuthorService(IGenericRepository<Author> repo)
    {
        private readonly IGenericRepository<Author> _repo = repo;

        public async Task<List<AuthorDto?>> GetAuthors()
        {
            var Authors = await _repo.GetAllAsync(x => x.Books);

            var AuthorsDto = Authors.Select(s => s.ToAuthorDto());

            return [.. AuthorsDto];
        }

        public async Task<AuthorDto?> GetAuthor(int id)
        {
            var isExists = await _repo.IsExistAsync(id);
            if (!isExists)
                return null;
            var author = await _repo.GetAsync(id, x => x.Books);
            return author?.ToAuthorDto();
        }

        public async Task<AuthorDto?> CreateAuthor(Author author)
        {
            var createdAuthor = await _repo.CreateAsync(author);
            return createdAuthor?.ToAuthorDto();
        }

        public async Task<AuthorDto?> UpdateAuthor(int id, UpdateAuthorRequestDto authorDto)
        {
            var isExists = await _repo.IsExistAsync(id);
            if (!isExists)
                return null;

            var originalAuthor = await _repo.GetAsync(id);
            originalAuthor.Name = authorDto.Name;
            originalAuthor.Bio = authorDto.Bio;

            var Author = await _repo.UpdateAsync(originalAuthor);

            return Author?.ToAuthorDto();
        }

        public async Task<bool> DeleteAuthor(int id)
        {
            var isExists = await _repo.IsExistAsync(id);
            if (!isExists)
                return false;
            await _repo.DeleteAsync(id);
            return true;
        }
    }
}
