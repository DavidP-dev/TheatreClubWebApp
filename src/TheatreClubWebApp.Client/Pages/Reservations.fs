module TheatreClubWebApp.Client.Pages.Reservations

open Feliz
open Feliz.DaisyUI
open TheatreClubWebApp.Client.Server

[<ReactComponent>]
let ReservationsView () =
        let reservations, setReservations = React.useState(List.empty)

        let loadReservations () = async {
            let! reservations = serviceR.GetReservations()
            setReservations reservations
        }
        React.useEffectOnce(loadReservations >> Async.StartImmediate)

        let memberRows =
            reservations
            |> List.map (fun r ->
                Html.tr [
                    Html.td r.Title
                    Html.td r.Theatre
                    Html.td r.DateAndTime
                    Html.td r.Cost
                    Html.td r.Genres
                    Html.td "2"
                    Html.td "Editovat / Smazat"
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
                            ]
                    ]
                ]
                Daisy.table [
                    prop.className "w-full"
                    prop.children [
                        Html.thead [Html.tr [Html.th ""; Html.th "Divadelní přestavení"; Html.th "Datum a čas představení"; Html.th "Objednatel"; Html.th "Zaplaceno"; Html.th "Vstupenky doručeny"; Html.th "Editace představení"]]

                    ]
                ]
            ]
        ]