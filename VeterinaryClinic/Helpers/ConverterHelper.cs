using System;
using VeterinaryClinic.Data.Entities;
using VeterinaryClinic.Models;
using VeterinaryClinic.Repositories;

namespace VeterinaryClinic.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
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
        public Room ToRoom(RoomViewModel model, bool isNew)
        {

            return new Room
            {
                Id = isNew ? 0 : model.Id,
                RoomNumber = model.RoomNumber,
                Type = model.Type,
                Vet = model.Vet,
                Status= model.Status,

            };

        }
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
    }
}
