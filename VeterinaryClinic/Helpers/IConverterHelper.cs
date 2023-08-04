using System;
using VeterinaryClinic.Data.Entities;
using VeterinaryClinic.Models;
using VeterinaryClinic.Repositories;

namespace VeterinaryClinic.Helpers
{
    public interface IConverterHelper
    {
        Customer ToCustomer(CustomerViewModel model, Guid imageId, bool isNew);

        CustomerViewModel ToCustomerViewModel(Customer customer);

        Pet ToPet(PetViewModel model, Guid imageId, bool isNew);

        PetViewModel ToPetViewModel(Pet pet);

        Vet ToVet(VetViewModel model, Guid imageId, bool isNew);

        VetViewModel ToVetViewModel(Vet vet);

        Room ToRoom(RoomViewModel model, bool isNew);

        RoomViewModel ToRoomViewModel(Room room);

        AppointmentDetailTemp ToAppointmentDetailTemp(AppointmentViewModel model);

        AppointmentViewModel ToAppointmentViewModel(AppointmentDetailTemp appointment);

    }
}
