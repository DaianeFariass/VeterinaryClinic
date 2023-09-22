using System;
using VeterinaryClinic.Data.Entities;
using VeterinaryClinic.Models;

namespace VeterinaryClinic.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        /// <summary>
        /// Método que retorna um novo customer.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="imageId"></param>
        /// <param name="isNew"></param>
        /// <returns>Customer</returns>
        public Customer ToCustomer(CustomerViewModel model, Guid imageId, bool isNew)
        {
            return new Customer
            {
                Id = isNew ? 0 : model.Id,
                ImageId = imageId,
                Name = model.Name,
                Document = model.Document,
                Address = model.Address,
                Phone = model.Phone,
                Email = model.Email,
                User = model.User,
            };
        }
        /// <summary>
        /// Método que retorna uma nova CustomerViewModel.
        /// </summary>
        /// <param name="customer"></param>
        /// <returns>Model</returns>
        public CustomerViewModel ToCustomerViewModel(Customer customer)
        {
            return new CustomerViewModel
            {
                Id = customer.Id,
                ImageId = customer.ImageId,
                Name = customer.Name,
                Document = customer.Document,
                Address = customer.Address,
                Phone = customer.Phone,
                Email = customer.Email,
                User = customer.User,

            };
        }
        /// <summary>
        /// Método que retorna um novo employee.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="imageId"></param>
        /// <param name="isNew"></param>
        /// <returns>Employee</returns>
        public Employee ToEmployee(EmployeeViewModel model, Guid imageId, bool isNew)
        {
            return new Employee
            {
                Id = isNew ? 0 : model.Id,
                ImageId = imageId,
                Name = model.Name,
                Address = model.Address,
                Phone = model.Phone,
                Email = model.Email,
                Role = model.Role,
                User = model.User,

            };
        }
        /// <summary>
        /// Método que retorna uma nova EmployeeViewModel.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="imageId"></param>
        /// <param name="isNew"></param>
        /// <returns>Model</returns>
        public EmployeeViewModel ToEmployeeViewModel(Employee employee)
        {
            return new EmployeeViewModel
            {
                Id = employee.Id,
                ImageId = employee.ImageId,
                Name = employee.Name,
                Address = employee.Address,
                Phone = employee.Phone,
                Email = employee.Email,
                Role = employee.Role,
                User = employee.User,

            };

        }
        /// <summary>
        /// Método que retorna um novo pet.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="imageId"></param>
        /// <param name="isNew"></param>
        /// <returns>Pet</returns>
        public Pet ToPet(PetViewModel model, Guid imageId, bool isNew)
        {
            return new Pet
            {
                Id = isNew ? 0 : model.Id,
                ImageId = imageId,
                Name = model.Name,
                DateOfBirth = model.DateOfBirth,
                Type = model.Type,
                Gender = model.Gender,
                Customer = model.Customer,

            };
        }
        /// <summary>
        /// Método que retorna uma nova PetViewModel.
        /// </summary>
        /// <param name="pet"></param>
        /// <returns>Model</returns>
        public PetViewModel ToPetViewModel(Pet pet)
        {
            return new PetViewModel
            {
                Id = pet.Id,
                ImageId = pet.ImageId,
                Name = pet.Name,
                DateOfBirth = pet.DateOfBirth,
                Type = pet.Type,
                Gender = pet.Gender,
                Customer = pet.Customer,

            };

        }
        /// <summary>
        /// Método que retorna um novo vet.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="imageId"></param>
        /// <param name="isNew"></param>
        /// <returns>Vet</returns>
        public Vet ToVet(VetViewModel model, Guid imageId, bool isNew)
        {
            return new Vet
            {
                Id = isNew ? 0 : model.Id,
                ImageId = imageId,
                Name = model.Name,
                Address = model.Address,
                Phone = model.Phone,
                Email = model.Email,
                Speciality = model.Speciality,
                User = model.User,

            };
        }
        /// <summary>
        /// Método que retorna uma nova VetViewModel.
        /// </summary>
        /// <param name="vet"></param>
        /// <returns>Model</returns>
        public VetViewModel ToVetViewModel(Vet vet)
        {
            return new VetViewModel
            {
                Id = vet.Id,
                ImageId = vet.ImageId,
                Name = vet.Name,
                Address = vet.Address,
                Phone = vet.Phone,
                Email = vet.Email,
                Speciality = vet.Speciality,
                User = vet.User,
            };

        }
        /// <summary>
        /// Método que retorna uma nova Room.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="isNew"></param>
        /// <returns>Room</returns>
        public Room ToRoom(RoomViewModel model, bool isNew)
        {

            return new Room
            {
                Id = isNew ? 0 : model.Id,
                RoomNumber = model.RoomNumber,
                Type = model.Type,
                Vet = model.Vet,
                Status = model.Status,

            };

        }
        /// <summary>
        /// Método que retorna uma nova RoomViewModel.
        /// </summary>
        /// <param name="room"></param>
        /// <returns>Model</returns>
        public RoomViewModel ToRoomViewModel(Room room)
        {

            return new RoomViewModel
            {
                Id = room.Id,
                RoomNumber = room.RoomNumber,
                Type = room.Type,
                Vet = room.Vet,
                VetId = room.Vet.Id,
                Status = room.Status,


            };

        }
        /// <summary>
        /// Método que retorna um novo appointmentDetailTemp.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="isNew"></param>
        /// <returns>AppointmentDetailTemp</returns>
        public AppointmentDetailTemp ToAppointmentDetailTemp(AppointmentDetailsViewModel model, bool isNew)
        {
            return new AppointmentDetailTemp
            {
                Id = isNew ? 0 : model.Id,
                User = model.User,
                Vet = model.Vet,
                Pet = model.Pet,
                Date = model.Date,
                Time = model.Time,

            };

        }
        /// <summary>
        /// Método que retorna uma nova AppointmentDetailsViewModel.
        /// </summary>
        /// <param name="appointment"></param>
        /// <returns>Model</returns>
        public AppointmentDetailsViewModel ToAppointmentDetailsViewModel(AppointmentDetailTemp appointment)
        {
            return new AppointmentDetailsViewModel
            {
                User = appointment.User,
                Pet = appointment.Pet,
                Vet = appointment.Vet,
                PetId = appointment.Pet.Id,
                VetId = appointment.Vet.Id,
                Date = appointment.Date,
                Time = appointment.Time,

            };
        }
        /// <summary>
        /// Método que retorna um novo appointment.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="isNew"></param>
        /// <returns>Appointment</returns>
        public Appointment ToAppointment(AppointmentViewModel model, bool isNew)
        {
            return new Appointment
            {
                Id = isNew ? 0 : model.Id,
                User = model.User,
                Vet = model.Vet,
                Pet = model.Pet,
                Date = model.Date,
                Time = model.Time,

            };

        }
        /// <summary>
        /// Método que retorna uma nova AppointmentViewModel.
        /// </summary>
        /// <param name="appointment"></param>
        /// <returns>Model</returns>
        public AppointmentViewModel ToAppointmentViewModel(Appointment appointment)
        {
            return new AppointmentViewModel
            {
                User = appointment.User,
                Pet = appointment.Pet,
                Vet = appointment.Vet,
                PetId = appointment.Pet.Id,
                VetId = appointment.Vet.Id,
                Date = appointment.Date,
                Time = appointment.Time,

            };
        }
        /// <summary>
        /// Método que retorna um novo pet report.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="isNew"></param>
        /// <returns>Pet Report</returns>
        public PetReport ToPetReport(PetReportViewModel model, bool isNew)
        {
            return new PetReport
            {
                Id = isNew ? 0 : model.Id,
                Vet = model.Vet,
                Pet = model.Pet,
                TestName = model.TestName,
                Diagnose = model.Diagnose,
                MedicineName = model.MedicineName
            };
        }
        /// <summary>
        /// Método que retorna uma nova PetViewModel.
        /// </summary>
        /// <param name="petReport"></param>
        /// <returns>Model</returns>
        public PetReportViewModel ToPetReportViewModel(PetReport petReport)
        {
            return new PetReportViewModel
            {

                Vet = petReport.Vet,
                Pet = petReport.Pet,
                TestName = petReport.TestName,
                Diagnose = petReport.Diagnose,
                MedicineName = petReport.MedicineName
            };
        }
        /// <summary>
        /// Método que retorna uma nova bill.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="isNew"></param>
        /// <returns>Bill</returns>
        public Bill ToBill(BillViewModel model, bool isNew)
        {
            return new Bill
            {
                Id = isNew ? 0 : model.Id,
                Appointment = model.Appointment,
                Cost = model.Cost,
                User = model.User,
            };
        }
        /// <summary>
        /// Método que retorna uma nova BillViewModel.
        /// </summary>
        /// <param name="bill"></param>
        /// <returns>Model</returns>
        public BillViewModel ToBillViewModel(Bill bill)
        {
            return new BillViewModel
            {
                Id = bill.Id,
                Appointment = bill.Appointment,
                Cost = bill.Cost,
                User = bill.User,
            };

        }

    }
}
