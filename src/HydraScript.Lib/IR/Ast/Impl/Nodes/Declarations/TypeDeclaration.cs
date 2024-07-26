using HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions.PrimaryExpressions;

namespace HydraScript.Lib.IR.Ast.Impl.Nodes.Declarations;

[AutoVisitable<AbstractSyntaxTreeNode>]
public partial class TypeDeclaration : Declaration
{
    private readonly TypeValue _typeValue;
    public IdentifierReference TypeId { get; }

    public TypeDeclaration(IdentifierReference typeId, TypeValue typeValue)
    {
        TypeId = typeId;
        _typeValue = typeValue;
    }

    public Type BuildType() =>
        _typeValue.BuildType(SymbolTable);

    public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
    {
        yield break;
    }

    protected override string NodeRepresentation() =>
        $"type {TypeId.Name} = {_typeValue}";
}