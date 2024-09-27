import { ServiceType } from "./service-type.model"

export interface Reservation {
  id?: number,
  userId?: number,
  serviceType?: ServiceType
  serviceTypeId?: number,
  dateOfReservation?: Date,
  dateTimeOfEndingService?: Date
}
