using Employee.Models;
using Employee.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Employee.Repository
{
    interface IService<T, TValue , TDatatype>
    {
        public void Create(TValue viewModel);
        public Task CreateAsync(TValue viewModel);

        public List<T> GetAll();
        public Task<List<T>> GetAllAsync();

        public T GetByID(TDatatype Id);
        public Task<T> GetByIDAsync(TDatatype Id);


        public void Update(TValue viewModel);
        public Task UpdateAsync(TValue viewModel);

        public void Delete(T Model);
        public Task DeleteAsync(T Model);
    }

    interface IUnitOfWork : IDisposable
    {
        void EnsureCreated();
        Task<int> SaveChangesAsync();
        EmployeeRepository EmployeesRepository { get; }
        DepartmentRepository DepartmentsRepository { get; }

    }

    public class UnitOfWork : IUnitOfWork
    {
        readonly EmpDepartmentDbContext Db;
        public UnitOfWork(EmpDepartmentDbContext _Db)
        {
            Db = _Db;
        }

        public EmployeeRepository EmployeesRepository => new EmployeeRepository(Db);
        public DepartmentRepository DepartmentsRepository => new DepartmentRepository(Db);

        public void Dispose()
        {
            Db.Dispose();
        }

        public void EnsureCreated()
        {
            Db.Database.EnsureCreated();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await Db.SaveChangesAsync();
        }
    }

    public class EmployeeRepository : IService<Employees, EmployeeViewModel, int>
    {
        public EmpDepartmentDbContext CurrentDbContext { get; private set; }
        public EmployeeRepository(EmpDepartmentDbContext db) 
        {
            CurrentDbContext = db;
        }

        public void Create(EmployeeViewModel viewModel)
        {
            CurrentDbContext.Set<Models.Employees>().Add(new Models.Employees
            {
                FirstName = viewModel.FirstName , LastName = viewModel.LastName , Age = viewModel.Age , Email = viewModel.Email ,
                PhoneNumber = viewModel.PhoneNumber , DepartmentIDFK = viewModel.DepartmentIDFK
            });
            CurrentDbContext.SaveChanges();
        }

        public async Task CreateAsync(EmployeeViewModel viewModel)
        {
            CurrentDbContext.Set<Models.Employees>().Add(new Models.Employees
            {
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
                Age = viewModel.Age,
                Email = viewModel.Email,
                PhoneNumber = viewModel.PhoneNumber,
                DepartmentIDFK = viewModel.DepartmentIDFK
            });
            await CurrentDbContext.SaveChangesAsync();
        }

        public void Delete(Models.Employees Model)
        {
            CurrentDbContext.Employee.Remove(Model);
            CurrentDbContext.SaveChanges();
        }

        public async Task DeleteAsync(Models.Employees Model)
        {
            CurrentDbContext.Employee.Remove(Model);
            await CurrentDbContext.SaveChangesAsync();
        }

        public List<Models.Employees> GetAll()
        {
            return CurrentDbContext.Employee.ToList();
        }

        public async Task<List<Models.Employees>> GetAllAsync()
        {
            return await CurrentDbContext.Employee.ToListAsync();
        }

        public Models.Employees GetByID(int Id)
        {
            return CurrentDbContext.Employee.Include(x => x.DepartmentFK).SingleOrDefault(x => x.Id == Id);
        }

        public async Task<Models.Employees> GetByIDAsync(int Id)
        {
            return await CurrentDbContext.Employee.Include(x => x.DepartmentFK).SingleOrDefaultAsync(x => x.Id == Id);
        }

        public void Update(EmployeeViewModel viewModel)
        {
            var DataOfEmployee = CurrentDbContext.Employee.Find(viewModel.Id);

            if(DataOfEmployee != null)
            {
                DataOfEmployee.FirstName = viewModel.FirstName == null ? DataOfEmployee.FirstName : viewModel.FirstName;
                DataOfEmployee.LastName = viewModel.LastName == null ? DataOfEmployee.LastName : viewModel.LastName;
                DataOfEmployee.PhoneNumber = viewModel.PhoneNumber == null ? DataOfEmployee.PhoneNumber : viewModel.PhoneNumber;
                DataOfEmployee.Age = viewModel.Age == 0 ? DataOfEmployee.Age : viewModel.Age;
                DataOfEmployee.Email = viewModel.Email == null ? DataOfEmployee.Email : viewModel.Email;
                DataOfEmployee.DepartmentIDFK = viewModel.DepartmentIDFK == 0 ? DataOfEmployee.DepartmentIDFK : viewModel.DepartmentIDFK;

                CurrentDbContext.Set<Models.Employees>().Update(DataOfEmployee);
                CurrentDbContext.SaveChanges();
            }
        }

        public async Task UpdateAsync(EmployeeViewModel viewModel)
        {
            var DataOfEmployee = await CurrentDbContext.Employee.FindAsync(viewModel.Id);

            if (DataOfEmployee != null)
            {
                DataOfEmployee.FirstName = viewModel.FirstName == null ? DataOfEmployee.FirstName : viewModel.FirstName;
                DataOfEmployee.LastName = viewModel.LastName == null ? DataOfEmployee.LastName : viewModel.LastName;
                DataOfEmployee.PhoneNumber = viewModel.PhoneNumber == null ? DataOfEmployee.PhoneNumber : viewModel.PhoneNumber;
                DataOfEmployee.Age = viewModel.Age == null ? DataOfEmployee.Age : viewModel.Age;
                DataOfEmployee.Email = viewModel.Email == null ? DataOfEmployee.Email : viewModel.Email;
                DataOfEmployee.DepartmentIDFK = viewModel.DepartmentIDFK == null ? DataOfEmployee.DepartmentIDFK : viewModel.DepartmentIDFK;

                CurrentDbContext.Set<Models.Employees>().Update(DataOfEmployee);
                await CurrentDbContext.SaveChangesAsync();
            }
        }
    }

    public class DepartmentRepository
    {
        public EmpDepartmentDbContext CurrentDbContext { get; private set; }
        public DepartmentRepository(EmpDepartmentDbContext db)
        {
            CurrentDbContext = db;
        }

        public Models.Department GetByID(int Id)
        {
            return CurrentDbContext.Department.Find(Id);
        }

        public async Task<Models.Department> GetByIDAsync(int Id)
        {
            return await CurrentDbContext.Department.FindAsync(Id);
        }

        public List<Models.Department> GetAll()
        {
            return CurrentDbContext.Department.ToList();
        }

        public async Task<List<Models.Department>> GetAllAsync()
        {
            return await CurrentDbContext.Department.ToListAsync();
        }
    }
}
