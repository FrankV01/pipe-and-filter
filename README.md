# Pipe and Filter example
This was a grad-school project put together as an assignment to demonstrate the
improvement that the pipe and filter architecture can bring for data processing.

If you're not familiar or I'm referring to it incorrectly, this is an
architectural pattern where a data stream comes in (in a stack or something) and
a thread takes an item off the stack, does some processing with it and puts it in
to a new stack. That next stack is then takes said item off the stack and does
further processing. This occurs until the final output.

`Start --> Step 1 --> Step 2 --> Step 3 -- Final output/ result.`

Each step is implemented as it's own thread to maximize throughput.

# Status
This was a long completed assignment which I keep up for my own reference and
well as an example for others.

I don't intend to further develop the project but should you have an improvement,
I'll gladly accept pull requests and such. Should you run in to a problem
try to read it or get it working, create an issue and I'll do my best to help
or patch the issue in a timely fashion.
