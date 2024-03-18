# Introduction

So, you have developed a command line tool, and now you want to distribute it in a way that is convenient for your end users. You also want to ensure the installation process is smooth, whether done manually or through an installation script in a CI/CD pipeline. Luckily, .NET provides a great solution for both of these challenges.

I've developed a basic CLI tool for this exercise that **computes the nth zero-based Fibonacci number**. The final outcome in the terminal will be a straightforward command `fibonacci` with the input argument `--iterations`, which takes the number of iterations to calculate.

Once installed, one can open up the terminal and call it: `fibonacci --iterations 3` which would produce:

```bash
PS C:\Users\User> fibonacci --iterations 3
The Fibonacci number for 3 iterations is 2
```

Said that, let's dive into producing such a tool...


# More Articles
If you enjoyed this articles, maybe you will enjoy more at [dinkopavicic.com](www.dinkopavicic.com)
