﻿using System;
using System.Text;
using System.Windows.Forms;
using Nikse.SubtitleEdit.Logic;
using System.Collections.Generic;

namespace Nikse.SubtitleEdit.Forms
{
    public sealed partial class FormRemoveTextForHearImpaired : Form
    {
        Subtitle _subtitle;
        readonly LanguageStructure.RemoveTextFromHearImpaired _language;

        public FormRemoveTextForHearImpaired()
        {
            InitializeComponent();

            _language = Configuration.Settings.Language.RemoveTextFromHearImpaired;

            Text = _language.Title;
            groupBoxRemoveTextConditions.Text = _language.RemoveTextConditions;
            labelAnd.Text = _language.And;
            labelRemoveTextBetween.Text = _language.RemoveTextBetween;
            checkBoxRemoveTextBeforeColon.Text = _language.RemoveTextBeforeColon;
            checkBoxRemoveTextBeforeColonOnlyUppercase.Text = _language.OnlyIfTextIsUppercase;
            checkBoxOnlyIfInSeparateLine.Text = _language.OnlyIfInSeparateLine;
            checkBoxRemoveTextBetweenBrackets.Text = _language.Brackets;
            checkBoxRemoveTextBetweenParentheses.Text = _language.Parentheses;
            checkBoxRemoveTextBetweenQuestionMarks.Text = _language.QuestionMarks;
            checkBoxRemoveTextBetweenSquares.Text = _language.SquareBrackets;
            checkBoxRemoveWhereContains.Text = _language.RemoveTextIfContains;            
            listViewFixes.Columns[0].Text = Configuration.Settings.Language.General.Apply;
            listViewFixes.Columns[1].Text = _language.LineNumber;
            listViewFixes.Columns[2].Text = _language.Before;
            listViewFixes.Columns[3].Text = _language.After;
            buttonOK.Text = Configuration.Settings.Language.General.OK;
            buttonCancel.Text = Configuration.Settings.Language.General.Cancel;
        }

        public static string RemoveStartEndNoise(string text)
        {
            string s = text.Trim();
            if (s.StartsWith("<b>") && s.Length > 3)
                s = s.Substring(3);
            if (s.StartsWith("<i>") && s.Length > 3)
                s = s.Substring(3);
            if (s.StartsWith("<u>") && s.Length > 3)
                s = s.Substring(3);
            if (s.StartsWith("<B>") && s.Length > 3)
                s = s.Substring(3);
            if (s.StartsWith("<I>") && s.Length > 3)
                s = s.Substring(3);
            if (s.StartsWith("<U>") && s.Length > 3)
                s = s.Substring(3);

            if (s.EndsWith("</b>") && s.Length > 4)
                s = s.Substring(0, s.Length-4);
            if (s.EndsWith("</i>") && s.Length > 4)
                s = s.Substring(0, s.Length-4);
            if (s.EndsWith("</u>") && s.Length > 4)
                s = s.Substring(0, s.Length-4);
            if (s.EndsWith("</B>") && s.Length > 4)
                s = s.Substring(0, s.Length-4);
            if (s.EndsWith("</I>") && s.Length > 4)
                s = s.Substring(0, s.Length-4);
            if (s.EndsWith("</U>") && s.Length > 4)
                s = s.Substring(0, s.Length-4);

            if (s.StartsWith("-") && s.Length > 2)
                s = s.TrimStart('-');
    
            return s.Trim();
        }

        public bool HasHearImpariedTagsAtStart(string text)
        {
            if (checkBoxOnlyIfInSeparateLine.Checked)
                return StartAndEndsWithHearImpariedTags(text);
            
            return (text.StartsWith("[") && text.IndexOf("]") > 0 && checkBoxRemoveTextBetweenSquares.Checked) ||
                   (text.StartsWith("{") && text.IndexOf("}") > 0 && checkBoxRemoveTextBetweenBrackets.Checked) ||
                   (text.StartsWith("?") && text.IndexOf("?", 1) > 0 && checkBoxRemoveTextBetweenQuestionMarks.Checked) ||
                   (text.StartsWith("(") && text.IndexOf(")") > 0 && checkBoxRemoveTextBetweenParentheses.Checked) ||
                   (text.StartsWith("[") && text.IndexOf("]:") > 0 && checkBoxRemoveTextBetweenSquares.Checked) ||
                   (text.StartsWith("{") && text.IndexOf("}:") > 0 && checkBoxRemoveTextBetweenBrackets.Checked) ||
                   (text.StartsWith("?") && text.IndexOf("?:", 1) > 0 && checkBoxRemoveTextBetweenQuestionMarks.Checked) ||
                   (text.StartsWith("(") && text.IndexOf("):") > 0 && checkBoxRemoveTextBetweenParentheses.Checked) ||
                   (checkBoxRemoveTextBetweenCustomTags.Checked &&
                    comboBoxCustomStart.Text.Length > 0 && comboBoxCustomEnd.Text.Length > 0 &&
                    text.StartsWith(comboBoxCustomStart.Text) && text.IndexOf(comboBoxCustomEnd.Text, 1) > 0);
        }

        public bool HasHearImpariedTagsAtEnd(string text)
        {
            if (checkBoxOnlyIfInSeparateLine.Checked)
                return StartAndEndsWithHearImpariedTags(text);

            return (text.EndsWith("]") && text.IndexOf("[") > 0 && checkBoxRemoveTextBetweenSquares.Checked) ||
                   (text.EndsWith("}") && text.IndexOf("{") > 0 && checkBoxRemoveTextBetweenBrackets.Checked) ||
                   (text.EndsWith(")") && text.IndexOf("(") > 0 && checkBoxRemoveTextBetweenParentheses.Checked) ||
                   (checkBoxRemoveTextBetweenCustomTags.Checked &&
                    comboBoxCustomStart.Text.Length > 0 && comboBoxCustomEnd.Text.Length > 0 &&
                    text.EndsWith(comboBoxCustomEnd.Text) && text.IndexOf(comboBoxCustomStart.Text) > 0);
        }


        private bool StartAndEndsWithHearImpariedTags(string text)
        {
            return (text.StartsWith("[") && text.EndsWith("]") && checkBoxRemoveTextBetweenSquares.Checked) ||
                   (text.StartsWith("{") && text.EndsWith("}") && checkBoxRemoveTextBetweenBrackets.Checked) ||
                   (text.StartsWith("?") && text.EndsWith("?") && checkBoxRemoveTextBetweenQuestionMarks.Checked) ||
                   (text.StartsWith("(") && text.EndsWith(")") && checkBoxRemoveTextBetweenParentheses.Checked) ||
                   (text.StartsWith("[") && text.EndsWith("]:") && checkBoxRemoveTextBetweenSquares.Checked) ||
                   (text.StartsWith("{") && text.EndsWith("}:") && checkBoxRemoveTextBetweenBrackets.Checked) ||
                   (text.StartsWith("?") && text.EndsWith("?:") && checkBoxRemoveTextBetweenQuestionMarks.Checked) ||
                   (text.StartsWith("(") && text.EndsWith("):") && checkBoxRemoveTextBetweenParentheses.Checked) ||
                   (checkBoxRemoveTextBetweenCustomTags.Checked &&
                    comboBoxCustomStart.Text.Length > 0 && comboBoxCustomEnd.Text.Length > 0 &&
                    text.StartsWith(comboBoxCustomStart.Text) && text.EndsWith(comboBoxCustomEnd.Text));
        }

        public void Initialize(Subtitle subtitle)
        {
            if (Environment.OSVersion.Version.Major < 6) // 6 == Vista/Win2008Server/Win7
            {
                string unicodeFontName = "Lucida Sans Unicode";
                float fontSize = comboBoxCustomStart.Font.Size;
                comboBoxCustomStart.Font = new System.Drawing.Font(unicodeFontName, fontSize);
                comboBoxCustomEnd.Font = new System.Drawing.Font(unicodeFontName, fontSize);
                comboBoxRemoveIfTextContains.Font = new System.Drawing.Font(unicodeFontName, fontSize);
            }
            comboBoxRemoveIfTextContains.Left = checkBoxRemoveWhereContains.Left + checkBoxRemoveWhereContains.Width;

            _subtitle = subtitle;
            GeneratePreview();
        }

        private void GeneratePreview()
        {
            listViewFixes.BeginUpdate();
            listViewFixes.Items.Clear();
            int count = 0;
            foreach (Paragraph p in _subtitle.Paragraphs)
            {
                bool hit = false;

                if (HasHearImpariedTagsAtStart(RemoveStartEndNoise(p.Text)))
                    hit = true;

                if (HasHearImpariedTagsAtEnd(RemoveStartEndNoise(p.Text)))
                    hit = true;

                if (!hit && checkBoxRemoveWhereContains.Checked && comboBoxRemoveIfTextContains.Text.Length > 0 && p.Text.Contains(comboBoxRemoveIfTextContains.Text))
                    hit = true;
                
                if (!hit)
                {
                    foreach (string s in p.Text.Trim().Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (HasHearImpariedTagsAtStart(RemoveStartEndNoise(s)))
                            hit = true;
                    }
                }

                if (RemoveColon(p.Text) != p.Text.Trim())
                {
                    hit = true;
                }

                if (hit)
                {
                    count++;
                    string newText = RemoveTextFromHearImpaired(p.Text);
                    AddToListView(p, newText);
                }
            }
            listViewFixes.EndUpdate();
            groupBoxLinesFound.Text = string.Format(_language.LinesFoundX, count);
        }

        private string RemoveColon(string text)
        {
            // House 7x01 line 52: and she would like you to do three things:
            // Okay or remove???
            if (text.IndexOf(':') > 0 && text.IndexOf(':') == text.Length - 1 && text != text.ToUpper())
                return text;

            List<int> removedAtLine = new List<int>();
            var sb = new StringBuilder();
            string[] parts = text.Trim().Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            // two ppl talking - add a "- " before text
            if (parts.Length == 2 && parts[0].Contains(": ") && parts[1].Contains(": ") && !text.Contains("-"))
            {
                parts[0] = parts[0].Replace(": ", ": - ");
                parts[1] = parts[1].Replace(": ", ": - ");
            }

            int lineNo = 1;
            foreach (string s in parts)
            {
                string s2 = s;
                if (checkBoxRemoveTextBeforeColon.Checked &&
                    (s2.Contains(": ") || s2.EndsWith(":")))
                {
                    if (!s2.StartsWith(s2.Substring(0, s2.IndexOf(":")).ToUpper()) &&
                        checkBoxRemoveTextBeforeColonOnlyUppercase.Checked)
                    {
                        sb.AppendLine(s2);
                    }
                    else if (s2.Contains(": "))
                    {
                        string textToRemove = s2.Substring(0, s2.IndexOf(": ") + 2);
                        if (textToRemove.Contains("<i>"))
                        {
                            s2 = "<i>" + s2.Remove(0, s2.IndexOf(": ") + 2);
                        }
                        else
                        {
                            s2 = s2.Remove(0, s2.IndexOf(": ") + 2);
                        }
                        removedAtLine.Add(lineNo);

                        if (!s2.Trim().StartsWith("-") &&
                            sb.ToString().Length == 0 &&
                            text.Contains(Environment.NewLine + "- "))
                            s2 = "- " + s2;

                        if (!s2.Trim().StartsWith("-") &&
                            sb.ToString().Length > 0 &&
                            sb.ToString().StartsWith("- "))
                            s2 = "- " + s2;

                        for (int k = 0; k < 2; k++)
                        {
                            if (s2.Contains(":"))
                            {
                                int colonIndex = s2.IndexOf(":");
                                string start = s2.Substring(0, colonIndex);
                                int periodIndex = start.LastIndexOf(".");
                                int questIndex = start.LastIndexOf("?");
                                int exclaIndex = start.LastIndexOf("!");
                                int endIndex = periodIndex;
                                if (endIndex == -1 || questIndex > endIndex)
                                    endIndex = questIndex;
                                if (endIndex == -1 || exclaIndex > endIndex)
                                    endIndex = exclaIndex;
                                if (endIndex > 0)
                                {
                                    s2 = s2.Remove(endIndex + 1, colonIndex - endIndex);
                                }
                            }
                        }

                        sb.AppendLine(s2);
                    }
                    else if (s2.Contains("!") || s2.Contains("?") || s2.Contains("."))
                    {
                        int index = s2.LastIndexOf("!");
                        if (index == -1 || s2.LastIndexOf("?") > index)
                            index = s2.LastIndexOf("?");
                        if (index == -1 || s2.LastIndexOf(".") > index)
                            index = s2.LastIndexOf(".");
                        if (index > 3)
                            sb.AppendLine(s2.Substring(0, index+1));
                    }
                }
                else
                {
                    sb.AppendLine(s2);
                }
                lineNo++;
            }

            string newText = sb.ToString().Trim();
            if (text != newText)
            {
                parts = newText.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 2)
                {
                    if (removedAtLine.Count > 0)
                    {
                        StripableText p0 = new StripableText(parts[0]);

                        if (p0.Post.Contains(".") || p0.Post.Contains("!") || p0.Post.Contains("?"))
                        {
                            StripableText p1 = new StripableText(parts[1]);
                            if (!p0.Pre.Contains("-"))
                                parts[0] = "- " + parts[0];

                            if (!p1.Pre.Contains("-"))
                                parts[1] = "- " + parts[1];
                        }
                    }

                    newText = parts[0] + Environment.NewLine +  parts[1];
                }
            }
            return newText;
        }

        private string RemoveTextFromHearImpaired(string text)
        {
            if (checkBoxRemoveWhereContains.Checked && comboBoxRemoveIfTextContains.Text.Length > 0 && text.Contains(comboBoxRemoveIfTextContains.Text))
            {
                return string.Empty;
            }

            string oldText = text;
            text = RemoveColon(text);
            StripableText st = new StripableText(text, " >-\"'‘`´♪¿¡.", " -\"'`´♪.!?:");
            var sb = new StringBuilder();
            string[] parts = st.StrippedText.Trim().Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in parts)
            {
                StripableText stSub = new StripableText(s, " >-\"'‘`´♪¿¡.", " -\"'`´♪.!?:");
                if (!StartAndEndsWithHearImpariedTags(stSub.StrippedText))
                {
                    string newText = stSub.StrippedText;
                    if (HasHearImpariedTagsAtStart(newText))
                        newText = RemoveStartEndTags(newText);
                    if (HasHearImpariedTagsAtEnd(newText))
                        newText = RemoveEndTags(newText);
                    sb.AppendLine(stSub.Pre + newText + stSub.Post);
                }
                else if (st.Pre.Contains("<i>") && stSub.Post.Contains("</i>"))
                {
                    st.Pre = st.Pre.Replace("<i>", string.Empty);
                }
            }

            text = st.Pre + sb.ToString().Trim() + st.Post;
            text = RemoveColon(text);

            st = new StripableText(text, " >-\"'‘`´♪¿¡.", " -\"'`´♪.!?:");
            text = st.StrippedText;
            if (StartAndEndsWithHearImpariedTags(text))
            {
                string newText = text;
                text = RemoveStartEndTags(text);
            }
            if (!string.IsNullOrEmpty(text))
                text = st.Pre + text + st.Post;

            if (oldText.Trim().StartsWith("- ") &&
                (oldText.Contains(Environment.NewLine + "- ") || oldText.Contains(Environment.NewLine + " - ")) &&
                text != null && !text.Contains(Environment.NewLine))
            {
                text = text.TrimStart().TrimStart('-').TrimStart();
            }
            return text;
        }

        private string RemoveEndTags(string newText)
        {
            string s = newText;

            if (s.EndsWith("]") && s.LastIndexOf("[") > 0 && checkBoxRemoveTextBetweenSquares.Checked)
                newText = s.Remove(s.LastIndexOf("["));

            else if (s.EndsWith("}") && s.LastIndexOf("{") > 0 && checkBoxRemoveTextBetweenBrackets.Checked)
                newText = s.Remove(s.LastIndexOf("}"));

            else if (s.EndsWith(")") && s.LastIndexOf("(") > 0 && checkBoxRemoveTextBetweenParentheses.Checked)
                newText = s.Remove(s.LastIndexOf("("));

            else if (checkBoxRemoveTextBetweenCustomTags.Checked &&
                     s.Length > 0 && comboBoxCustomEnd.Text.Length > 0 && comboBoxCustomStart.Text.Length > 0 &&
                     s.EndsWith(comboBoxCustomEnd.Text) && s.LastIndexOf(comboBoxCustomStart.Text) > 0)
                newText = s.Remove(s.LastIndexOf(comboBoxCustomStart.Text));

            if (newText != s)
                newText = newText.TrimEnd(' ');
            return newText;
        }

        private string RemoveStartEndTags(string text)
        {
            string newText = text;
            string s = text;
            if (s.StartsWith("[") && s.IndexOf("]") > 0 && checkBoxRemoveTextBetweenSquares.Checked)
                newText = s.Remove(0, s.IndexOf("]") + 1);
            else if (s.StartsWith("{") && s.IndexOf("}") > 0 && checkBoxRemoveTextBetweenBrackets.Checked)
                newText = s.Remove(0, s.IndexOf("}") + 1);
            else if (s.StartsWith("?") && s.IndexOf("?", 1) > 0 && checkBoxRemoveTextBetweenQuestionMarks.Checked)
                newText = s.Remove(0, s.IndexOf("?", 1) + 1);
            else if (s.StartsWith("(") && s.IndexOf(")") > 0 && checkBoxRemoveTextBetweenParentheses.Checked)
                newText = s.Remove(0, s.IndexOf(")") + 1);
            else if (s.StartsWith("[") && s.IndexOf("]:") > 0 && checkBoxRemoveTextBetweenSquares.Checked)
                newText = s.Remove(0, s.IndexOf("]:") + 2);
            else if (s.StartsWith("{") && s.IndexOf("}:") > 0 && checkBoxRemoveTextBetweenBrackets.Checked)
                newText = s.Remove(0, s.IndexOf("}:") + 2);
            else if (s.StartsWith("?") && s.IndexOf("?:", 1) > 0 && checkBoxRemoveTextBetweenQuestionMarks.Checked)
                newText = s.Remove(0, s.IndexOf("?:") + 2);
            else if (s.StartsWith("(") && s.IndexOf("):") > 0 && checkBoxRemoveTextBetweenParentheses.Checked)
                newText = s.Remove(0, s.IndexOf("):") + 2);
            else if (checkBoxRemoveTextBetweenCustomTags.Checked &&
                     s.Length > 0 && comboBoxCustomEnd.Text.Length > 0 && comboBoxCustomStart.Text.Length > 0 &&
                     s.StartsWith(comboBoxCustomStart.Text) && s.LastIndexOf(comboBoxCustomEnd.Text) > 0)
                newText = s.Remove(0, s.LastIndexOf(comboBoxCustomEnd.Text) + comboBoxCustomEnd.Text.Length);
            if (newText != text)
                newText = newText.TrimStart(' ');
            return newText;
        }

        private void AddToListView(Paragraph p, string newText)
        {
            var item = new ListViewItem(string.Empty) {Tag = p, Checked = true};

            var subItem = new ListViewItem.ListViewSubItem(item, p.Number.ToString());
            item.SubItems.Add(subItem);
            subItem = new ListViewItem.ListViewSubItem(item, p.Text.Replace(Environment.NewLine, Configuration.Settings.General.ListViewLineSeparatorString));
            item.SubItems.Add(subItem);
            subItem = new ListViewItem.ListViewSubItem(item, newText.Replace(Environment.NewLine, Configuration.Settings.General.ListViewLineSeparatorString));
            item.SubItems.Add(subItem);

            listViewFixes.Items.Add(item);
        }

        private void FormRemoveTextForHearImpaired_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                DialogResult = DialogResult.Cancel;
        }

        private void ButtonOkClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        public int RemoveTextFromHearImpaired()
        {
            int count = 0;
            for (int i = _subtitle.Paragraphs.Count - 1; i >= 0; i--)
            {
                Paragraph p = _subtitle.Paragraphs[i];
                if (IsFixAllowed(p))
                {
                    string newText = RemoveTextFromHearImpaired(p.Text);
                    if (string.IsNullOrEmpty(newText))
                    {
                        _subtitle.Paragraphs.RemoveAt(i);
                    }
                    else
                    {
                        p.Text = newText;
                    }
                    count++;
                }
            }
            return count;
        }

        private bool IsFixAllowed(Paragraph p)
        {
            foreach (ListViewItem item in listViewFixes.Items)
            {
                if (item.Tag.ToString() == p.ToString())
                    return item.Checked;
            }
            return false;
        }

        private void CheckBoxRemoveTextBetweenCheckedChanged(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            GeneratePreview();
            Cursor = Cursors.Default;
        }

    }
}
