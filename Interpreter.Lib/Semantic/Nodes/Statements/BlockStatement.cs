using System.Collections.Generic;
using System.Linq;
using Interpreter.Lib.IR.Instructions;

namespace Interpreter.Lib.Semantic.Nodes.Statements
{
    public class BlockStatement : Statement
    {
        private readonly List<StatementListItem> statementList;

        public BlockStatement(IEnumerable<StatementListItem> statementList)
        {
            this.statementList = new List<StatementListItem>(statementList);
            this.statementList.ForEach(item => item.Parent = this);
        }

        public bool HasReturnStatement()
        {
            var has = statementList.Any(item => item is ReturnStatement);
            if (!has)
            {
                has = statementList
                    .Where(item => item.IsStatement())
                    .OfType<IfStatement>()
                    .Any(ifStmt => ifStmt.HasReturnStatement());
            }

            return has;
        }

        public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator() => statementList.GetEnumerator();

        protected override string NodeRepresentation() => "{}";

        public override List<Instruction> ToInstructions(int start)
        {
            var blockInstructions = new List<Instruction>();
            var offset = start;
            foreach (var item in statementList)
            {
                var itemInstructions = item.ToInstructions(offset);
                blockInstructions.AddRange(itemInstructions);
                if (item is ReturnStatement)
                {
                    break;
                }

                offset += itemInstructions.Count;
            }

            return blockInstructions;
        }
    }
}