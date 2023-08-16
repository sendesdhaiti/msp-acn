using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACTIONS.STYLE
{
    public class STYLES
    {

        public string display_flex_justify_n_align_center_direction_row_inline =
            "display: flex; justify-content: center; align-content: center; flex-direction: row;";

        public string display_flex_justify_n_align_center_direction_column_inline =
            "display: flex; justify-content: center; align-content: center; flex-direction: column; ";

        public string border_style_dot5rem_rad = 
        "border:.5px solid black;border-radius: .5rem;padding:5px;margin: 5px auto 5px auto;";

        public string transitionMixin =
            @"
            @mixin transitionMixin{
                @media screen and (max-width: 500px) {
                    transition: 2s cubic-bezier(0.075, 0.82, 0.165, 1);
                }
                @media screen and (min-width: 500px) {
                    transition: 2s cubic-bezier(0.1, 0.82, 0.165, 1);
                }
            }
        ";

        public string all =
            @"*{
            justify-content: center;
            align-content: center;
            border: .5px dotted;
            border-radius: .5rem;
            padding:5px;
            margin: 2px 2px 2px 2px;
            all:unset;
        }
        * > span{
            all:unset;
        }
        
        ";

        public static string background1 = "rgb(246, 233, 51)";
        public static string background2 = "rgb(51, 184, 246)";
        public static string background3 = "rgb(69, 195, 60)";
        public static string background4 = "rgb(251, 75, 196)";

        
        public static string transitionMixin_inline =
            "@media screen and (max-width: 500px) {"
            + "transition: 2s cubic-bezier(0.075, 0.82, 0.165, 1);"
            + "}"
            + "@media screen and (min-width: 500px) {"
            + "transition: 2s cubic-bezier(0.1, 0.82, 0.165, 1);"
            + "}";

        public string columnToRowMixin_inline =
            "@media screen and (max-width: 500px) {"
            + "flex-direction: column;"
            + "}"
            + "@media screen and (min-width: 500px) {"
            + "flex-direction: row;"
            + "}";

        public string btn1Styles_inline =
            $"@include {transitionMixin_inline};"
            + $"@include {background3Styles_inline};"
            + "background-color: black;"
            + "color: yellow;"
            + "padding: 1rem;"
            + "margin: 10px;"
            + "border: 1px solid;"
            + "border-radius: 1rem;";

        public static string background1Styles_inline =
            "@media screen and (max-width: 500px) {"
            + ".background1_1{transition: 2s cubic-bezier(0.075, 0.82, 0.165, 1);"
            + $"background-color: {background1};"
            + "}}"
            + "@media screen and (min-width: 500px) {"
            + ".background1_2{transition: 2s cubic-bezier(0.1, 0.82, 0.165, 1);"
            + $"background-color: {background2};"
            + "}}";


        public string p = "p{font-size:small;}";
        public string background1_style = background1Styles_inline;

        public static string background2Styles_inline =
            "@media screen and (max-width: 500px) {"
            + "transition: 2s cubic-bezier(0.075, 0.82, 0.165, 1);"
            + $"background-color: {background2};"
            + "}"
            + "@media screen and (min-width: 500px) {"
            + "transition: 2s cubic-bezier(0.1, 0.82, 0.165, 1);"
            + $"background-color: {background3};"
            + "}";

        public static string background3Styles_inline =
            "@media screen and (max-width: 500px) {"
            + "transition: 2s cubic-bezier(0.075, 0.82, 0.165, 1);"
            + $"background-color: {background1};"
            + "}"
            + "@media screen and (min-width: 500px) {"
            + "transition: 2s cubic-bezier(0.1, 0.82, 0.165, 1);"
            + $"background-color: {background4}"
            + "}";
    }
}
