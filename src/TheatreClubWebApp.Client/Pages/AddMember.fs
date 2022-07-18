module TheatreClubWebApp.Client.Pages.AddMember

open System
open Feliz
open Feliz.DaisyUI
open TheatreClubWebApp.Client.Server


[<ReactComponent>]

let addMemberView () =
    Html.div [prop.text "Pro přidání člena vyplň formulář a stiskni tlačítko přidat člena."]