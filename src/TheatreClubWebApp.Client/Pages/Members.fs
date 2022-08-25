module TheatreClubWebApp.Client.Pages.Members

open System
open Feliz
open Feliz.DaisyUI
open TheatreClubWebApp.Client.Server
open TheatreClubWebApp.Client.Router
open TheatreClubWebApp.Shared.Domain

let private toCzech (genre: Genre) : string =
        match genre with
        | Alternative -> "Alternativa"
        | Art -> "Umění"
        | Comedy -> "Komedie"
        | Dance -> "Taneční"
        | Drama -> "Drama"
        | Mainstream -> "Mejnstrým"
        | Musical -> "Muzikál"
        | Philosophy -> "Filosofie"


[<ReactComponent>]
let MembersView () =
        let members, setMembers = React.useState(List.empty)

        let loadMembers () = async {
            let! members = service.GetClubMembers()
            setMembers members
        }
        React.useEffectOnce(loadMembers >> Async.StartImmediate)

        let delete = React.useCallback(fun i ->
            async {
                let! _ = service.DeleteClubMember i
                let! _ = loadMembers ()
                return ()
            }
            |> Async.StartImmediate)

        let memberRows =
            members
            |> List.map (fun m ->
                Html.tr [
                    Html.td m.Surname
                    Html.td m.Name
                    Html.td m.Email
                    Html.td (String.Join(", ", (List.map toCzech m.PreferredGenres)))
                    Html.td m.NumberOfReservedTickets
                    Html.td [
                        Daisy.button.button  [
                            prop.className "btn-sm"
                            button.outline
                            button.primary
                            prop.text "Editovat"
                            prop.onClick (fun _ -> m.Id |> Page.EditMember |> Router.navigatePage)
                        ]
                    ]
                    Html.td [
                        Daisy.button.button  [
                            prop.className "btn-sm"
                            button.outline
                            button.primary
                            prop.text "Smazat"
                            prop.onClick (fun _ -> delete m.Id)
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
                            prop.text "Přidej člena"
                            prop.onClick (fun _ -> Page.AddMember |> Router.navigatePage)
                            ]
                    ]
                ]
                Daisy.table [
                    prop.className "w-full"
                    prop.children [
                        Html.thead [Html.tr [
                            Html.th "Příjmení"
                            Html.th "Jméno"
                            Html.th "Email"
                            Html.th "Preferované žánry"
                            Html.th "Počet rezervovaných vstupenek"
                            Html.th "Editace člena"
                            Html.th "Smazání člena"
                        ]]
                        Html.tbody memberRows
                    ]
                ]
            ]
        ]