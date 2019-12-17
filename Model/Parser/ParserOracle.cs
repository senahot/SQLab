using SQLab.Model.Parser.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLab.Model.Parser
{
    public class ParserOracle
    {
        private readonly string originalSQL;
        private readonly Dictionary<char, int> charDictionary;

        /// <summary>
        /// Not a real parser, just a validator
        /// </summary>
        /// <param name="sql">sql statement to be validated</param>
        public ParserOracle(string sql)
        {
            this.originalSQL = sql;
            this.charDictionary = this.originalSQL
                                  .GroupBy(c => c)
                                  .ToDictionary(grp => grp.Key, grp => grp.Count());

            this.Parse();
        }

        public List<ParserError> Errors { get; } = new List<ParserError>();
        public QueryTree QueryTree { get; private set; }

        private void Parse()
        {
            string sql = this.ValidateStrings();
            if (string.IsNullOrEmpty(sql)) return;
            sql = ValidateBrackets(sql);
            this.QueryTree = this.CreateQueryTree(sql);
            //quebrar em espaços
            //identificar statements
        }

        private string ValidateStrings()
        {
            if (this.charDictionary.TryGetValue('\'', out int n))
            {
                if (n % 2 != 0)
                {
                    int index = this.originalSQL.IndexOf('\'');
                    ParserError error = new ParserError(index, ParserErrorType.NOT_CLOSED);
                    this.Errors.Add(error);

                    return string.Empty;
                }

                return ReturnSQLWhithoutStrings();
            }

            return this.originalSQL;
        }

        private string ReturnSQLWhithoutStrings()
        {
            string[] noTextString = this.originalSQL.Split('\'');
            StringBuilder sb = new StringBuilder();
            bool jump = false;
            foreach (string stg in noTextString)
            {
                if (!jump)
                {
                    sb.Append(stg);
                }

                jump = !jump;
            }

            return sb.ToString();
        }

        private string ValidateBrackets(string noTextSQL)
        {
            if (this.charDictionary.TryGetValue('(', out int openBrackets))
            {
                this.charDictionary.TryGetValue(')', out int closebrackets);

                if (openBrackets > closebrackets)
                {
                    int index = this.originalSQL.IndexOf('(');
                    ParserError error = new ParserError(index, ParserErrorType.NOT_CLOSED);
                    this.Errors.Add(error);
                }
                else if (closebrackets > openBrackets)
                {
                    int index = this.originalSQL.LastIndexOf(')');
                    ParserError error = new ParserError(index, ParserErrorType.NOT_OPENED);
                    this.Errors.Add(error);
                }
            }

            return this.originalSQL;
        }

        private QueryTree CreateQueryTree(string validSQL)
        {
            QueryTree queryTree = new QueryTree();
            if (this.charDictionary.TryGetValue('(', out _))
            {
                queryTree = this.CreateQueryTreeRecursivaly(validSQL, new List<QueryTree>(), 0);
            }
            else
            {
                QueryTree.Query = validSQL;
            }


            return queryTree;
        }

        private QueryTree CreateQueryTreeRecursivaly(string validSQL, List<QueryTree> queryTrees, int iteration)
        {
            int lastOpenBracket = validSQL.LastIndexOf('(');
            if (lastOpenBracket != -1)
            {
                QueryTree qTree = this.TextToQueryTree(validSQL, lastOpenBracket);
                queryTrees = this.AddChildrensOrBrothers(queryTrees, qTree);

                int numberOfBrackets = 2;
                string text = validSQL.Remove(lastOpenBracket, qTree.Query.Length + numberOfBrackets);

                return this.CreateQueryTreeRecursivaly(text, queryTrees, iteration + 1);
            }
            else
            {
                QueryTree queryTree = this.TextToQueryTree(validSQL, -1, validSQL.Length - 1);

                if (queryTrees.Count != 0)
                {
                    queryTree.Childrens.AddRange(queryTrees);
                }

                return queryTree;
            }
        }

        private List<QueryTree> AddChildrensOrBrothers(List<QueryTree> queryTrees, QueryTree qTree)
        {
            if (queryTrees.Count > 0)
            {
                bool isMyBrother = qTree.IsInsideMyInterval(queryTrees[0]);
                if (isMyBrother)
                {
                    queryTrees.Add(qTree);
                }
                else
                {
                    qTree.Childrens.AddRange(queryTrees);
                    queryTrees = new List<QueryTree>() { qTree };
                }
            }
            else
            {
                queryTrees = new List<QueryTree>() { qTree };
            }

            return queryTrees;
        }

        private QueryTree TextToQueryTree(string validSQL, int lastOpenBracket, int closeBracketIndex)
        {
            string sql = validSQL.Substring(lastOpenBracket + 1, closeBracketIndex);

            QueryTree qTree = new QueryTree
            {
                Query = sql,
                StartIndex = lastOpenBracket,
                EndIndex = closeBracketIndex
            };

            return qTree;
        }

        private QueryTree TextToQueryTree(string validSQL, int lastOpenBracket)
        {
            int initialIndex = lastOpenBracket + 1;
            string s = validSQL.Substring(initialIndex);
            int closeBracketIndex = s.IndexOf(')');

            return TextToQueryTree(validSQL, lastOpenBracket, closeBracketIndex);
        }

        private string IdentifyStatement(string validSQL)
        {
            return "";
        }
    }
}