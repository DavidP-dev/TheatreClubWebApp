module TheatreClubWebApp.Client.Pages.Members

open System
open Feliz
open Feliz.DaisyUI


type MemberWeb =
     {
     Id : Guid
     Name : string
     Surname : string
     Email : string
     PreferredGenres : string
     }

[<ReactComponent>]
let MembersView () =
        React.fragment[

            Daisy.button.button [
                button.outline
                button.primary
                button.lg
                prop.text "Přidej člena"
            ]

            Daisy.table [
                prop.className "w-full"
                prop.children [
                    Html.thead [Html.tr [Html.th ""; Html.th "Jméno"; Html.th "Email"; Html.th "Preferované žánry"]]
                    Html.tbody [Html.tr [Html.td "1"; Html.td "David Pícha"; Html.td "picha.mda@seznam.cz"; Html.td "Umění, Filozofie, Komedie"]]
                    Html.tbody [Html.tr [Html.td "2"; Html.td "Petra Dvořáčková"; Html.td "tloustnurychle@seznam.cz"; Html.td "Komedie, Taneční"]]
                    Html.tbody [Html.tr [Html.td "3"; Html.td "Karel Ferjentsik"; Html.td "karelnahrad@seznam.cz"; Html.td "Filozofie, Taneční"]]
                ]
            ]
        ]