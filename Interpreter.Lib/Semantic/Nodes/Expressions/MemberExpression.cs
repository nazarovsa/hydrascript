using System.Collections.Generic;
using Interpreter.Lib.IR.Instructions;
using Interpreter.Lib.Semantic.Exceptions;
using Interpreter.Lib.Semantic.Nodes.Expressions.AccessExpressions;
using Interpreter.Lib.Semantic.Nodes.Expressions.PrimaryExpressions;
using Interpreter.Lib.Semantic.Symbols;
using Interpreter.Lib.Semantic.Types;

namespace Interpreter.Lib.Semantic.Nodes.Expressions
{
    public class MemberExpression : Expression
    {
        private readonly IdentifierReference _id;
        
        public AccessExpression AccessChain { get; }

        public MemberExpression(IdentifierReference id, AccessExpression accessChain)
        {
            _id = id;
            _id.Parent = this;
            
            AccessChain = accessChain;
            if (accessChain != null)
            {
                AccessChain.Parent = this;
            }
        }

        public string Id => _id.Id;

        internal override Type NodeCheck()
        {
            if (AccessChain == null)
            {
                return _id.NodeCheck();
            }

            var symbol = SymbolTable.FindSymbol<VariableSymbol>(_id.Id);
            if (symbol == null)
            {
                throw new UnknownIdentifierReference(_id);
            }

            return AccessChain.Check(symbol.Type);
        }

        public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
        {
            if (AccessChain != null)
            {
                yield return AccessChain;
            }
        }

        protected override string NodeRepresentation() => Id;

        public override List<Instruction> ToInstructions(int start, string temp)
        {
            if (AccessChain != null && AccessChain.HasNext())
            {
                return AccessChain.ToInstructions(start, _id.Id);
            }

            return new();
        }

        public static implicit operator IdentifierReference(MemberExpression member) => 
            member._id;
        
        public static explicit operator MemberExpression(IdentifierReference idRef) =>
            new (idRef, null);
    }
}