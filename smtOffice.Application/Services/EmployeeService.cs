using AutoMapper;
using smtOffice.Application.DTOs;
using smtOffice.Application.Interfaces.Repository;
using smtOffice.Application.Interfaces.Services;
using smtOffice.Domain.Entity;
using System.Reflection;

namespace smtOffice.Application.Services
{
    public class EmployeeService(IEmployeeRepository employeeRepository, IMapper mapper) : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository = employeeRepository;
        private readonly IMapper _mapper = mapper;

        public async Task CreateEmployeeAsync(EmployeeDTO employee)
        {
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee));
            }
            var employeeDto = _mapper.Map<Employee>(employee);

            await _employeeRepository.CreateEmployeeAsync(employeeDto);
        }
        public async Task<IEnumerable<EmployeeDTO>> ReadEmployeesAsync()
        {
            var employees = await _employeeRepository.ReadEmployeesAsync();
            var employeeDTOs = _mapper.Map<IEnumerable<EmployeeDTO>>(employees);

            return employeeDTOs;
        }
        public async Task<List<EmployeeDTO>> ReadEmployeeAsync(string position)
        {
            var employees = await _employeeRepository.ReadEmployeesAsync(position);
            var employeeDTOs = _mapper.Map<List<EmployeeDTO>>(employees);

            return employeeDTOs;
        }
        public async Task<EmployeeDTO> ReadEmployeeByUsernameAsync(string userName)
        {
            var employees = await _employeeRepository.ReadEmployeeAsync(userName);
            var employeeDTOs = _mapper.Map<EmployeeDTO>(employees);

            return employeeDTOs;
        }
        public async Task<EmployeeDTO> ReadEmployeeAsync(int employeeID)
        {
            var employees = await _employeeRepository.ReadEmployeeAsync(employeeID);
            var employeeDTOs = _mapper.Map<EmployeeDTO>(employees);

            return employeeDTOs;
        }
        public async Task UpdateEmployeeAsync(EmployeeDTO employeeDTO)
        {
            // Retrieve the current employee record
            var currentEmployee = await _employeeRepository.ReadEmployeeAsync(employeeDTO.ID);

            if (currentEmployee == null)
            {
                throw new ArgumentException($"No employee found with ID {employeeDTO.ID}");
            }

            // Get the properties of the EmployeeDTO
            var properties = typeof(EmployeeDTO).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                // Get the value of the property from the DTO
                var value = property.GetValue(employeeDTO);

                // Check if the value is not null or empty (for string properties)
                if (value != null && !(value is string str && string.IsNullOrEmpty(str)))
                {
                    // Get the corresponding property in the Employee object
                    var employeeProperty = typeof(Employee).GetProperty(property.Name, BindingFlags.Public | BindingFlags.Instance);

                    // If the property exists on the Employee object, set its value
                    if (employeeProperty != null && employeeProperty.CanWrite)
                    {
                        employeeProperty.SetValue(currentEmployee, value);
                    }
                }
            }

            await _employeeRepository.UpdateEmployeeAsync(currentEmployee);
        }

        public async Task DeleteEmployeeAsync(int employeeID)
        {
            await _employeeRepository.DeleteEmployeeAsync(employeeID);
        }
    }
}
