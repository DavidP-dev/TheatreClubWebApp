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
    | EditMember of Guid
    | EditPerformance of Guid
    | EditReservation of Guid

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
        | [ "editaceclena"; Route.Guid i ] -> Page.EditMember i
        | [ "editacepredstaveni"; Route.Guid i ] -> Page.EditPerformance i
        | [ "editacerezervace"; Route.Guid i ] -> Page.EditReservation i
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
        | Page.EditMember i -> [ "editaceclena"; string i ] |> noQueryString
        | Page.EditPerformance i -> [ "editacepredstaveni"; string i ] |> noQueryString
        | Page.EditReservation i -> [ "editacerezervace"; string i ] |> noQueryString

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