module TheatreClubWebApp.Client.Pages.ArchiveOfReservations


open Feliz
open Feliz.DaisyUI
open TheatreClubWebApp.Client.Server
open TheatreClubWebApp.Client.Router

let boolToHumanLanguage =
    function
        |true -> "Ano"
        |false -> "Ne"

[<ReactComponent>]
let ArchiveOfReservationsView () =
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
                    Html.td "Dělám na tom"
                    Html.td "Dělám na tom"
                    Html.td "Dělám na tom"
                    Html.td "Dělám na tom"
                    Html.td "Dělám na tom"
                    Html.td "Dělám na tom"
                    Html.td [
                        Daisy.button.button  [
                            prop.className "btn-sm"
                            button.outline
                            button.primary
                            prop.text "Editovat"
                         //   prop.onClick (fun _ -> Page.EditReservation r.ReservationID |> Router.navigatePage)
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
                                          //  prop.onClick (fun _ -> delete r.ReservationID)
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