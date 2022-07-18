module TheatreClubWebApp.Client.Pages.Members

open System
open Feliz
open Feliz.DaisyUI
open TheatreClubWebApp.Client.Server
open TheatreClubWebApp.Client.Router

[<ReactComponent>]
let MembersView () =
        let members, setMembers = React.useState(List.empty)

        let loadMembers () = async {
            let! members = service.GetClubMembers()
            setMembers members
        }
        React.useEffectOnce(loadMembers >> Async.StartImmediate)

        let memberRows =
            members
            |> List.map (fun m ->
                Html.tr [
                    Html.td m.Surname
                    Html.td m.Name
                    Html.td m.Email
                    Html.td (String.Join(", ", m.PreferredGenres))
                    Html.td m.MemberReservations
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
                            prop.text "Přidej člena"
                            prop.onClick (fun _ -> Page.AddMember |> Router.navigatePage)
                            ]
                    ]
                ]
                Daisy.table [
                    prop.className "w-full"
                    prop.children [
                        Html.thead [Html.tr [Html.th "Příjmení"; Html.th "Jméno"; Html.th "Email"; Html.th "Preferované žánry"; Html.th "Aktivní rezervace"; Html.th "Editace člena"]]
                        Html.tbody memberRows
                    ]
                ]
            ]
        ]