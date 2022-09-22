module TheatreClubWebApp.Client.Pages.Reservations

open System
open Feliz
open Feliz.DaisyUI
open TheatreClubWebApp.Client.Server
open TheatreClubWebApp.Client.Router

module Transfers =
    let dateTimeOffsetToString (dto: DateTimeOffset) : string =
        let dayOfWeekInCzech =
            match dto.DayOfWeek with
            | DayOfWeek.Monday -> "Pondělí"
            | DayOfWeek.Tuesday -> "Úterý"
            | DayOfWeek.Wednesday -> "Středa"
            | DayOfWeek.Thursday -> "Čtvrtek"
            | DayOfWeek.Friday -> "Pátek"
            | DayOfWeek.Saturday -> "Sobota"
            | DayOfWeek.Sunday -> "Neděle"
            | _ -> failwith "Takový den neexistuje"

        let dateTimeString = dto.ToString("dd.MM.yyyy HH:mm")

        let dateTimeOffsetString = dayOfWeekInCzech + ", " + dateTimeString
        dateTimeOffsetString

let boolToHumanLanguage =
    function
        |true -> "Ano"
        |false -> "Ne"

[<ReactComponent>]
let ReservationsView () =
        let reservations, setReservations = React.useState(List.empty)

        let loadReservations () = async {
            let! reservations = serviceR.GetReservations()
            setReservations reservations
        }
        React.useEffectOnce(loadReservations >> Async.StartImmediate)

        let delete = React.useCallback(fun i ->
            async {
                let! _ = serviceR.DeleteReservation i
                let! _ = loadReservations ()
                return ()
            }
            |> Async.StartImmediate)

        let reservationsRows =
            reservations
            |> List.map (fun r ->
                Html.tr [
                    Html.td r.PerformanceTitle
                    Html.td (r.PerformanceDateAndTime |> Transfers.dateTimeOffsetToString)
                    Html.td (r.MemberName + " " +  r.MemberSurname)
                    Html.td r.NumberOfReservedTickets
                    Html.td (r.IsPaid |> boolToHumanLanguage)
                    Html.td (r.TicketsReceived |> boolToHumanLanguage)
                    Html.td [
                        Daisy.button.button  [
                            prop.className "btn-sm"
                            button.outline
                            button.primary
                            prop.text "Editovat"
                            prop.onClick (fun _ -> Page.EditReservation r.ReservationID |> Router.navigatePage)
                        ]
                        Daisy.button.label [
                            prop.htmlFor (r.ReservationID |> string)
                            prop.className "btn-sm"
                            button.outline
                            button.primary
                            prop.text "Smazat"
                            ]
                        Daisy.modalToggle [prop.id (r.ReservationID |> string)]
                        Daisy.modal [
                            prop.children [
                                Daisy.modalBox [
                                    Html.p $"Opravdu chceš smazat rezervaci na představení {r.PerformanceTitle}?"
                                    Daisy.modalAction [
                                        Daisy.button.label [
                                            prop.htmlFor (r.ReservationID |> string)
                                            button.primary
                                            prop.text "Ano"
                                            prop.onClick (fun _ -> delete r.ReservationID)
                                        ]
                                        Daisy.button.label [
                                            prop.htmlFor (r.ReservationID |> string)
                                            button.primary
                                            prop.text "Ne"
                                        ]
                                    ]
                                ]
                            ]
                        ]
                    ]
                ]
            )

        Html.div[
            prop.className "flex flex-col gap-4"
            prop.children [
                Html.div[
                    prop.className "flex justify-center"
                    prop.children[
                        Daisy.button.button [
                            button.outline
                            button.primary
                            button.lg
                            prop.text "Přidej rezervaci"
                            prop.onClick (fun _ -> Page.AddReservation |> Router.navigatePage)
                        ]
                        Daisy.button.button [
                            button.outline
                            button.primary
                            button.lg
                            prop.text "Archiv rezervací"
                            prop.onClick (fun _ -> Page.ArchiveOfReservations |> Router.navigatePage)
                        ]
                    ]
                ]
                Daisy.table [
                    prop.className "w-full"
                    prop.children [
                        Html.thead [
                            Html.tr [
                                Html.th "Divadelní přestavení"
                                Html.th "Datum a čas představení"
                                Html.th "Objednatel"
                                Html.th "Rezervované vstupenky"
                                Html.th "Zaplaceno"
                                Html.th "Doručeno"
                                Html.th "Editace / Smazání rezervace"
                            ]
                        ]
                        Html.tbody reservationsRows
                    ]
                ]
            ]
        ]