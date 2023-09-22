using System;
using VeterinaryClinic.Data.Entities;
using VeterinaryClinic.Models;

namespace VeterinaryClinic.Helpers
{
    public interface IConverterHelper
    {
        Customer ToCustomer(CustomerViewModel model, Guid imageId, bool isNew);

        CustomerViewModel ToCustomerViewModel(Customer customer);

        Employee ToEmployee(EmployeeViewModel model, Guid imageId, bool isNew);

        EmployeeViewModel ToEmployeeViewModel(Employee employee);

        Pet ToPet(PetViewModel model, Guid imageId, bool isNew);

        PetViewModel ToPetViewModel(Pet pet);

        Vet ToVet(VetViewModel model, Guid imageId, bool isNew);

        VetViewModel ToVetViewModel(Vet vet);

        Room ToRoom(RoomViewModel model, bool isNew);

        RoomViewModel ToRoomViewModel(Room room);

        Appointment ToAppointment(AppointmentViewModel model, bool isNew);

        AppointmentViewModel ToAppointmentViewModel(Appointment appointment);

        AppointmentDetailTemp ToAppointmentDetailTemp(AppointmentDetailsViewModel model, bool isNew);

        AppointmentDetailsViewModel ToAppointmentDetailsViewModel(AppointmentDetailTemp appointment);

        PetReport ToPetReport(PetReportViewModel model, bool isNew);

        PetReportViewModel ToPetReportViewModel(PetReport petReport);

        Bill ToBill(BillViewModel model, bool isNew);

        BillViewModel ToBillViewModel(Bill bill);

    }
}
