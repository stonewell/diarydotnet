using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.XPath;
using System.Collections;

namespace Diary.Net.Html2Rtf
{
    public static class Functions
    {
        public static double[] arrTotLen = new double[0];
        public static double[] arrMaxLen = new double[0];
        public static double multiplyWidth = 192;
        public static double marginWidth = 196;

        public static string TableCellWidthFill(XPathNodeIterator objXMLNodes,
            double tableWidth)
        {
            string strText = "";
            if (objXMLNodes != null && objXMLNodes.Count != 0)
            {
                objXMLNodes.MoveNext();
                XPathNodeIterator objRowNodes = objXMLNodes.Current.Select("./*");
                XPathNavigator objRowNode;
                XPathNodeIterator objColNodes;
                double maxWordLen;

                for (int i = 0; i < objRowNodes.Count; i++)
                {
                    objXMLNodes.MoveNext();
                    objRowNode = objRowNodes.Current;
                    objColNodes = objRowNode.Select("./*");

                    if (arrTotLen.Length < objColNodes.Count)
                    {
                        double[] tmp = new double[objColNodes.Count];
                        System.Array.Copy(arrTotLen, tmp, arrTotLen.Length);
                        arrTotLen = tmp;
                    }

                    if (arrMaxLen.Length < objColNodes.Count)
                    {
                        double[] tmp = new double[objColNodes.Count];
                        System.Array.Copy(arrMaxLen, tmp, arrMaxLen.Length);
                        arrMaxLen = tmp;
                    }

                    for (int j = 0; j < objColNodes.Count; j++)
                    {
                        objColNodes.MoveNext();
                        string[] arrWords = Regex.Split(objColNodes.Current.InnerXml, "/[\\s+]/");
                        maxWordLen = 0;
                        arrTotLen[j] = objColNodes.Current.Value.Length;
                        for (int iWord = 0; iWord < arrWords.Length; iWord++)
                        {
                            if (arrWords[iWord].Length > maxWordLen)
                            {
                                maxWordLen = arrWords[iWord].Length;
                            }
                        }
                        arrMaxLen[j] = arrMaxLen[j] > maxWordLen ? arrMaxLen[j] : (maxWordLen + 1);
                    }
                }

                if (tableWidth > 0)
                {
                    double totalWidthTot = 0;
                    double totalWidthMax = 0;

                    for (int i = 0; i < arrMaxLen.Length; i++)
                    {
                        totalWidthTot += (arrTotLen[i] * multiplyWidth + 2 * marginWidth);
                        totalWidthMax += (arrMaxLen[i] * multiplyWidth + 2 * marginWidth);
                    }

                    double tableWidthTot = tableWidth;
                    double tableWidthMax = tableWidth;
                    double midWidthTot = tableWidth / arrTotLen.Length;
                    double midWidthMax = tableWidth / arrMaxLen.Length;
                    double midAddTot = (tableWidth - totalWidthTot) / arrTotLen.Length;
                    double midAddMax = (tableWidth - totalWidthMax) / arrMaxLen.Length;

                    //strText += "totalWidthTot = " + totalWidthTot.toString() + " \n";
                    //strText += "totalWidthMax = " + totalWidthMax.toString() + " \n";
                    //strText += "midAddTot = " + midAddTot.toString() + " \n";
                    //strText += "midAddMax = " + midAddMax.toString() + " \n";
                    for (int i = 0; i < arrMaxLen.Length; i++)
                    {
                        arrMaxLen[i] = (arrMaxLen[i] * multiplyWidth + 2 * marginWidth) * tableWidth / totalWidthTot / multiplyWidth;
                    }
                }
            }
            return strText;
        }

        public static string CharCode(string strText)
        {
            StringBuilder strCharCodes = new StringBuilder();
            string strSeparator = "";

            strCharCodes.Append("###");

            for (int intChar = 0; intChar < strText.Length; intChar++)
            {
                strCharCodes.Append(char.ConvertToUtf32(strText, intChar));
                strCharCodes.Append(strSeparator);
                strSeparator = ",";
            }

            strCharCodes.Append("###");
            return strCharCodes.ToString();
        }

        public static string RTFEncode(XPathNodeIterator objXMLNodes,
            string strText, double intMyNormalizeSpaces)
        {
            // Encode text, character by character

            if (intMyNormalizeSpaces == 1)
            {
                // Replace multiple spaces by one single space
                //strText = Regex.Replace(strText, "/ +/g", " ");
            }

            bool blnAppendParagraphBreak = false;

            // Build an array of characters
            char[] arrChars = strText.ToCharArray();

            StringBuilder strRTFEncoded = new StringBuilder();

            for (int intChar = 0; intChar < arrChars.Length; intChar++)
            {
                char strChar = arrChars[intChar];
                switch (strChar.ToString())
                {
                    case "\\":
                    case "{":
                    case "}":
                        // Encode backslashes, left curly bracket, right curly bracket (prefix with a backslash)
                        strRTFEncoded.Append("\\").Append(strChar);
                        break;

                    case " ":
                        // Encode non-breacking space (backslash+tilda)
                        strRTFEncoded.Append("\\~"); ;
                        break;

                    case "\n":
                        if (intMyNormalizeSpaces == 2)
                        {
                            // Preformatted mode - use \line for all EOL characters
                            strRTFEncoded.Append("\\line ");

                            // Check if next node is a paragraph - if yes, we will use a paragraph break INSTEAD of line break
                            if (objXMLNodes != null && objXMLNodes.Count != 0)
                            {
                                objXMLNodes.MoveNext();
                                XPathNavigator objXMLContext = objXMLNodes.Current;
                                XPathNavigator objNextNode = objXMLContext.SelectSingleNode("following-sibling::node()[position() = 1]");
                                if (objNextNode != null)
                                {
                                    if (objNextNode.LocalName == "p")
                                    {
                                        blnAppendParagraphBreak = true;
                                    }
                                }
                            }
                        }
                        else
                        {
                            strRTFEncoded.Append(strChar);
                        }
                        break;

                    default:
                        int intCharCode = char.ConvertToUtf32(strChar.ToString(), 0);

                        if (intCharCode > 255)
                        {
                            // Non-ascii: encode as UNICODE (\u)
                            strRTFEncoded.Append("\\u").Append(intCharCode).Append("  ");
                        }
                        else
                        {
                            // TODO Handle control characters (ASCII code lesser than 32 - TAB, EOL, etc...)
                            // No encoding
                            strRTFEncoded.Append(strChar);
                        }
                        break;

                }
            }

            // Convert back array to string
            if (blnAppendParagraphBreak)
            {
                // Append a paragraph break - next node is a p tag, but we are not inside a p tag (bad!)
                strRTFEncoded.Append("\\par ");
            }

            return strRTFEncoded.ToString();
        }

        public static double GetTableColumnWidth(int iColumn, double bSum, double font_size)
        {
            multiplyWidth = font_size * 8;
            double sum = 0;
            iColumn = iColumn - 1;
            if (iColumn < arrMaxLen.Length && iColumn >= 0)
            {
                if (bSum == 0)
                {
                    return (2 * marginWidth + arrMaxLen[iColumn] * multiplyWidth);
                }
                else if (bSum == 1)
                {
                    return (2 * marginWidth + arrTotLen[iColumn] * multiplyWidth);
                }
                else if (bSum == 2)
                {
                    for (int i = 0; i <= iColumn; i++)
                    {
                        sum += (2 * marginWidth + arrMaxLen[i] * multiplyWidth);
                    }
                    return (sum);
                }
                else if (bSum == 3)
                {
                    for (int i = 0; i <= iColumn; i++)
                    {
                        sum += (2 * marginWidth + arrTotLen[i] * multiplyWidth);
                    }
                    return (sum);
                }
            }
            return 0;
        }

    }
}
