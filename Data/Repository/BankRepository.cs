using CrossCuttingConcerns.Generics;
using CrossCuttingConcerns.PagingSorting;
using Data.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{
    public interface IPersonRepository
    {
        IQueryable<Bank> GetAll();

        Task<PaginatedList<Bank>> GetList(
            int? pageNumber,
            string sortField,
            string sortOrder,
            int? pageSize);

        Task<Bank> GetById(int id);
        Task<Bank> GetByIdWithTracking(int id);
        Task<Bank> Create(Bank entity);
        Task Update(Bank entity);
        Task Delete(int id);
        
    }

    public class BankRepository : GenericRepository<Bank>, IPersonRepository
    {
        private readonly MainDbContext _mainDbContext;

        public BankRepository(MainDbContext mainDbContext) : base(mainDbContext)
        {
            _mainDbContext = mainDbContext;
        }

        public override async Task<Bank> GetById(int id)
        {
            return await GetAll()
                .
        }


        //Create
        public override async Task<Bank> Create(Bank entity)
        {
            var muncipality = entity.Address.Municipality;
            entity.Address.ArbeidsRegio = ArbeidsmarktregiosByMuncipality(muncipality);

            await _mainDbContext.Persons.AddAsync(entity);
            await _mainDbContext.SaveChangesAsync();
            return entity;
        }


        //Update
        public override async Task<Bank> Update(Bank entity)
        {
            _mainDbContext.Update(entity);
            await _mainDbContext.SaveChangesAsync();
            return entity;
        }


        //Delete
        public override async Task Delete(int id)
        {
            var personToDelete = await _mainDbContext.Persons.FirstOrDefaultAsync(p => p.Id == id);

            _mainDbContext.Persons.Remove(personToDelete);
            await _mainDbContext.SaveChangesAsync();
        }
    }

    
}
