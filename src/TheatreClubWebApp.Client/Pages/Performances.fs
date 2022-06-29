module TheatreClubWebApp.Client.Pages.Performances

open Feliz
open Feliz.DaisyUI

[<ReactComponent>]
let PerformancesView () =
    Html.div[

        Daisy.button.button [
            button.outline
            button.primary
            button.lg
            prop.text "Do NOT click on me!"
        ]

        Daisy.table [
            prop.className "w-full"
            prop.children [
                Html.thead [Html.tr [Html.th ""; Html.th "Name"; Html.th "Job"; Html.th "Favorite Color"]]
                Html.tbody [Html.tr [Html.td "1"; Html.td "Cy Ganderton"; Html.td "Quality Control Specialist"; Html.td "Blue"]]
                Html.tbody [Html.tr [Html.td "2"; Html.td "Hart Hagerty"; Html.td "Desktop Support Technician"; Html.td "Purple"]]
                Html.tbody [Html.tr [Html.td "3"; Html.td "Brice Swyre"; Html.td "Tax Accountant"; Html.td "Red"]]
                Html.tbody [Html.tr [Html.td "4"; Html.td "Marjy Ferencz"; Html.td "Office Assistant I"; Html.td "Crimson"]]
            ]
        ]
    ]