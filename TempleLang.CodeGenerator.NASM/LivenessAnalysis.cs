using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TempleLang.Intermediate;

namespace TempleLang.CodeGenerator.NASM
{
    public class LivenessAnalysis
    {
        public class CFGNode
        {
            public IInstruction Instruction { get; }

            public List<CFGNode> Successors { get; }

            public List<IReadableValue> Inp { get; }
            public List<IAssignableValue> Out { get; }

            public List<IReadableValue> Use { get; }
            public List<IAssignableValue> Def { get; }

            public CFGNode(IInstruction instruction, CFGNode? directSuccessor)
            {
                Inp = new List<IReadableValue>();
                Out = new List<IAssignableValue>();

                Use = new List<IReadableValue>();
                Def = new List<IAssignableValue>();

                switch (instruction)
                {
                    case BinaryComputationAssignment inst:
                        Def.Add(inst.Target);
                        Use.Add(inst.Lhs);
                        Use.Add(inst.Rhs);
                        break;

                    case UnaryComputationAssignment inst:
                        Def.Add(inst.Target);
                        Use.Add(inst.Operand);
                        break;

                    case ConditionalJump inst:
                        Use.Add(inst.Condition);
                        break;

                    case ReturnInstruction inst:
                        if (inst.ReturnValue != null) Use.Add(inst.ReturnValue);
                        break;

                    case ParameterQueryAssignment inst:
                        Def.Add(inst.Target);
                        break;

                    case CallInstruction inst:
                        Use.AddRange(inst.Parameters);
                        break;
                }

                Successors = directSuccessor == null ? new List<CFGNode>(1) : new List<CFGNode>(2) { directSuccessor };
            }
        }

        public IEnumerable<CFGNode> ConstructCFG(List<IInstruction> instructions)
        {
            CFGNode[] nodes = new CFGNode[instructions.Count];

            int end = instructions.Count - 1;
            for (int i = end; i >= 0; i--)
            {
                nodes[i] = new CFGNode(instructions[i], i == end ? null : nodes[i + 1]);
            }

            // Map Labels to instructions
            Dictionary<LabelInstruction, IInstruction> labels = new Dictionary<LabelInstruction, IInstruction>();

            List<LabelInstruction> activeLabels = new List<LabelInstruction>();

            foreach (var instruction in instructions)
            {
                if (instruction is LabelInstruction label)
                {
                    activeLabels.Add(label);
                }
                else if (activeLabels.Count > 0)
                {
                    foreach (var activeLabel in activeLabels)
                    {
                        labels[activeLabel] = instruction;
                    }
                }
            }

            foreach (var node in nodes)
            {
                if (node.Instruction is ConditionalJump conditionalJump)
                {
                    cfgNppde
                }
            }
        }
    }
}
