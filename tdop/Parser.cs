using System;
using System.Collections.Generic;

namespace tdop
{
    public class Parser
    {
        private int i = 0;
        private Dictionary<string, Symbol> symbolTable;

        private Token[] tokens;

        public Parser()
        {
            symbolTable = new Dictionary<string, Symbol>();

            GetSymbol("number", (t) => t);
        }


        public List<TreeNode> Parse(Token[] tokens)
        {
            this.tokens = tokens;
            var parseTree = new List<TreeNode>();
            
            while (currentToken().Type != "(end)")
            {
                parseTree.Add(Expresssion(0));
            }
            return parseTree;
        }

        private TreeNode currentToken()
        {
            return InterpretToken(tokens[i]);
        }

        private TreeNode advance()
        {
            i++;
            return currentToken();
        }


        TreeNode Expresssion(int rightPowerBinding)
        {
            TreeNode left;
            var t = currentToken();
            advance();
            if (t.NullDenotativeFunction == null)
                throw new ApplicationException("Unexpected token: " + t.Type);
            left = t.NullDenotativeFunction(t);
            while (rightPowerBinding < currentToken().LeftPowerBinding)
            {
                t = currentToken();
                advance();
                if(t.LeftDenotativeFunction == null)
                    throw new ApplicationException("Unexpected token: " + t.Type);
                left = t.LeftDenotativeFunction(left);
            }
            return left;

        }

        TreeNode InterpretToken(Token token)
        {
            Symbol symbol = GetSymbol(token.Type);
            return new TreeNode()
                       {
                           LeftDenotativeFunction = symbol.LeftDenotativeFunction,
                           LeftPowerBinding = symbol.LeftPowerBinding,
                           NullDenotativeFunction = symbol.NullDenotativeFunction,
                           Type = token.Type,
                           Value = token.Value
                        };
        }

        private Symbol GetSymbol(string type, Func<TreeNode, TreeNode> nud = null, int lbp = 0, Func<TreeNode, TreeNode> led = null)
        {
            Symbol symbol = null;
            if(!symbolTable.TryGetValue(type, out symbol))
            {
                symbol = new Symbol();
                symbolTable.Add(type, symbol);
            }

            if (lbp > 0) symbol.LeftPowerBinding = lbp;
            if (nud != null) symbol.NullDenotativeFunction = nud;
            if (led != null) symbol.LeftDenotativeFunction = led;

            return symbol;
        }
    }

    internal class Symbol
    {
        public int LeftPowerBinding { get; set; }
        public Func<TreeNode, TreeNode> NullDenotativeFunction { get; set; }
        public Func<TreeNode, TreeNode> LeftDenotativeFunction { get; set; }
    }


    public class TreeNode
    {
        public string Type { get; set; }

        public object Value { get; set; }

        public TreeNode Left { get; set; }

        public TreeNode Right { get; set; }

        public int LeftPowerBinding { get; set; }

        public Func<TreeNode, TreeNode> NullDenotativeFunction { get; set; }
        
        public Func<TreeNode, TreeNode> LeftDenotativeFunction { get; set; }


    }


    public class Token
    {
        public string Type { get; set; }
        public object Value { get; set; }
    }
    
}