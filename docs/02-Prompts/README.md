# Prompts

Crafting the right prompts is essential to elicit accurate and relevant responses from Language Models. Prompts serve as the input to the model, guiding it to generate the desired output. This section provides guidelines and best practices for creating effective prompts to interact with Language Models.

## Propmt Templates

The Semantic Kernel SDK provides a templating language that enables you to create prompts that can be reused. By using tokens, you can replace the input parameters of a prompt dynamically. Furthermore, you can call functions within the prompt to perform operations on the input parameters. To embed expressions in your prompts, the templating language uses curly brackets {{...}}.

## Tips for Crafting Prompts

**Specific Inputs Yield Specific Outputs**: Language Models respond based on the input they receive. Therefore, crafting clear and specific prompts is crucial to get the desired output.

**Experimentation is Key**: You may need to iterate and experiment with different prompts to understand how the model interprets and generates responses. Small tweaks can lead to significant changes in outcomes.

**Context Matters**: Language Models consider the context provided in the prompt. Therefore, you should ensure that the context is well-defined and relevant to obtain accurate and coherent responses.

**Handle Ambiguity**: Bear in mind that Language Models may struggle with ambiguous queries. Provide context or structure to avoid vague or unexpected results.

**Length of Prompts**: While Language Models can process both short and long prompts, you should consider the trade-off between brevity and clarity. Experimenting with prompt length can help you find the optimal balance.
