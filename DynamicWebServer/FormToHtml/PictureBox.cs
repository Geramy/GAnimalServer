﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using DynamicWebServer.special_html;

namespace DynamicWebServer.FormToHtml
{
    public class HTMLPictureBox : HtmlControl
    {
        public delegate byte[] ResponseRequested(string Variable, string Value);
        public event ResponseRequested OnResponseRequested;
        public CssScript cssScript = new CssScript();
        string HTMLCode = "";
        public HTMLPictureBox(System.Windows.Forms.PictureBox control)
            : base(control)
        {
            this.Thiscontrol = control;
            cssScript.html_Type = "div";
            cssScript.type_Name = Thiscontrol.Name;
            cssScript.sxml = special_xml.css;
            cssScript.mime_Type = type.css;
            cssScript.values = new string[] { (Thiscontrol.Bounds.X + (Thiscontrol.Bounds.X / 2)).ToString() + "px", (Thiscontrol.Bounds.Y + (Thiscontrol.Bounds.Y / 2)).ToString() + "px", "absolute", Thiscontrol.ClientRectangle.Height.ToString() + "px", Thiscontrol.ClientRectangle.Width.ToString() + "px" };
            cssScript.variables = new css_variables[] { css_variables.left, css_variables.top, css_variables.position, css_variables.height, css_variables.width };
            //this.Thiscontrol.ClientRectangle.X
            OnItemResponse += new FormItemResponse(HTMLTextBox_OnItemResponse);
        }
        byte[] HTMLTextBox_OnItemResponse(string Variable, string Value)
        {
            // Example button1_Click_1(sender, e);
            Button TempButton = (Button)(Thiscontrol);
            TempButton.PerformClick();
            return OnResponseRequested(Variable, Value);
        }
        public override string PrintAllHTML()
        {
            return base.PrintAllHTML();
        }
        public string GetPageCode(string CurrentPage)
        {
            string Javascript = "";
            string cssSRipt = Make_Special.MakeCssScript(cssScript);
            string TempStr = CurrentPage;
            if (TempStr.IndexOf("function ReloadPage()") == -1)
            {
                if ((string)(this.Thiscontrol.Tag) == "")
                {
                    Javascript = ""; //Make_Special.MakeJavaScript("ReloadPage", (object[])(new Enum[] { CommonJs.location_href }), new string[] { this.Thiscontrol.Text.Split('*')[1] + "?" + this.Thiscontrol.Name });
                }
                else
                {
                    Javascript = Make_Special.MakeJavaScript("ReloadPage" + this.Thiscontrol.Name, (object[])(new Enum[] { CommonJs.location_href }), new string[] { this.Thiscontrol.Tag + "?" + this.Thiscontrol.Name });
                }
            }

            if (TempStr.IndexOf("<body>") != -1)
            {
                int test = TempStr.IndexOf("<body>");
                if ((string)(this.Thiscontrol.Tag) != "")
                {

                    string BeforeBody = TempStr.Substring(0, (TempStr.IndexOf("<body>") - 1));
                    string AfterBodyCode = "<div class=" + this.Thiscontrol.Name + "><input class=" + this.Thiscontrol.Name + " type=button value=" + this.Thiscontrol.Text + " onClick=\"ReloadPage" + this.Thiscontrol.Name + "();\" /></div>" + "<body>";

                    string HtmlAfterCode = TempStr.Substring((TempStr.IndexOf("<body>") + 7));

                    TempStr = cssSRipt + Javascript + BeforeBody + AfterBodyCode + HtmlAfterCode;
                }
                else
                {

                    string BeforeBody = TempStr.Substring(0, TempStr.IndexOf("<body>") - 1);
                    string AfterBodyCode = "<div class=" + this.Thiscontrol.Name + "><input class=" + this.Thiscontrol.Name + " type=button value=" + this.Thiscontrol.Text + " onClick=\"index.html?"+this.Thiscontrol.Name+"=Clicked\" /></div>" + "<body>";
                    string HtmlAfterCode = TempStr.Substring(TempStr.IndexOf("<body>") + 7);
                    TempStr = cssSRipt + Javascript + BeforeBody + AfterBodyCode + HtmlAfterCode;

                }
            }
            return TempStr;
        }
    }
}
