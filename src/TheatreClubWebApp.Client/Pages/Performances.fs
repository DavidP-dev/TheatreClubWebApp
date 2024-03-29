module TheatreClubWebApp.Client.Pages.Performances

open System
open Feliz
open Feliz.DaisyUI
open TheatreClubWebApp.Client.Router
open TheatreClubWebApp.Client.Server

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


[<ReactComponent>]
let PerformancesView () =

        let performances, setPerformances = React.useState(List.Empty)

        let loadPerformances () = async {
            let! performances = serviceP.GetPerformances()
            setPerformances performances
        }
        React.useEffectOnce(loadPerformances >> Async.StartImmediate)

        let delete = React.useCallback(fun i ->
            async {
                let! _ = serviceP.DeletePerformance i
                let! _ = loadPerformances ()
                return ()
            }
            |> Async.StartImmediate)

        let performanceRows =
            performances
            |> List.map ( fun p ->
                    Html.tr [
                        Html.td p.Title
                        Html.td p.Theatre
                        Html.td (p.DateAndTime |> Transfers.dateTimeOffsetToString)
                        Html.td p.NumberOfAvailableTickets
                        Html.td p.NumberOfReservedTickets
                        Html.td (p.Cost + " " + "Kč")
                        Html.td [
                            Daisy.button.button  [
                                prop.className "btn-sm"
                                button.outline
                                button.primary
                                prop.text "Editovat"
                                prop.onClick (fun _ -> p.Id |> Page.EditPerformance |> Router.navigatePage)
                            ]
                            Daisy.button.label [
                                prop.htmlFor (p.Id |> string)
                                prop.className "btn-sm"
                                button.outline
                                button.primary
                                prop.text "Smazat"
                            ]
                            Daisy.modalToggle [prop.id (p.Id |> string)]
                            Daisy.modal [
                                prop.children [
                                    Daisy.modalBox [
                                        Html.p $"Opravdu chceš smazat představení s názvem {p.Title}?"
                                        Daisy.modalAction [
                                            Daisy.button.label [
                                                prop.htmlFor (p.Id |> string)
                                                button.primary
                                                prop.text "Ano"
                                                prop.onClick (fun _ -> delete p.Id)
                                            ]
                                            Daisy.button.label [
                                                prop.htmlFor (p.Id |> string)
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
                            prop.text "Přidej představení"
                            prop.onClick (fun _ -> Page.AddPerformance |> Router.navigatePage)
                        ]
                        Daisy.button.button [
                            button.outline
                            button.primary
                            button.lg
                            prop.text "Archiv představení"
                            prop.onClick (fun _ -> Page.ArchiveOfPerformances |> Router.navigatePage)
                        ]
                    ]
                ]
                Daisy.table [
                    prop.className "w-full"
                    prop.children [
                        Html.thead [Html.tr [Html.th "Divadelní představení"
                                             Html.th "Divadlo"; Html.th "Datum a čas"
                                             Html.th "Dostupné vstupenky"
                                             Html.th "Rezervované vstupenky"
                                             Html.th "Cena vstupenky"
                                             Html.th "Editace / Smazání představení"]]
                        Html.tbody performanceRows
                    ]
                ]
            ]
        ]