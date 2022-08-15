module TheatreClubWebApp.Client.Router

open Browser.Types
open Feliz.Router
open Fable.Core.JsInterop
open System

type Page =
    | Index
    | Members
    | Performances
    | Reservations
    | AddMember
    | AddPerformance
    | AddReservation
    | EditMember
    | EditPerformance of Guid
    | EditReservation

[<RequireQualifiedAccess>]
module Page =
    let defaultPage = Page.Index

    let parseFromUrlSegments = function
        | [ ] -> Page.Index
        | [ "clenove" ] -> Page.Members
        | [ "predstaveni" ] -> Page.Performances
        | [ "rezervace" ] -> Page.Reservations
        | [ "pridaniclena" ] -> Page.AddMember
        | [ "pridanipredstaveni" ] -> Page.AddPerformance
        | [ "pridanirezervace" ] -> Page.AddReservation
        | [ "editaceclena" ] -> Page.EditMember
        | [ "editacepredstaveni"; Route.Guid i ] -> Page.EditPerformance i
        | [ "editacerezervace" ] -> Page.EditReservation
        | _ -> defaultPage

    let noQueryString segments : string list * (string * string) list = segments, []

    let toUrlSegments = function
        | Page.Index -> [ ] |> noQueryString
        | Page.Members -> [ "clenove" ] |> noQueryString
        | Page.Performances -> [ "predstaveni" ] |> noQueryString
        | Page.Reservations -> [ "rezervace" ] |> noQueryString
        | Page.AddMember -> [ "pridaniclena" ] |> noQueryString
        | Page.AddPerformance -> [ "pridanipredstaveni" ] |> noQueryString
        | Page.AddReservation -> [ "pridanirezervace" ] |> noQueryString
        | Page.EditMember -> [ "editaceclena" ] |> noQueryString
        | Page.EditPerformance i -> [ "editacepredstaveni"; string i ] |> noQueryString
        | Page.EditReservation -> [ "editacerezervace" ] |> noQueryString

[<RequireQualifiedAccess>]
module Router =
    let goToUrl (e:MouseEvent) =
        e.preventDefault()
        let href : string = !!e.currentTarget?attributes?href?value
        Router.navigatePath href

    let navigatePage (p:Page) = p |> Page.toUrlSegments |> Router.navigatePath

[<RequireQualifiedAccess>]
module Cmd =
    let navigatePage (p:Page) = p |> Page.toUrlSegments |> Cmd.navigatePath