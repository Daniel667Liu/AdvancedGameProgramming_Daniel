using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BehaviorTree
{

    //create the basic abstract class node, all other nodes derive from this class
    public abstract class Node<T>
    {
        public abstract bool Update(T context);
    }

    //define the tree class
    public class Tree<T> : Node<T>
    {
        private readonly Node<T> _root;

        //pass the root node into readonly var
        public Tree(Node<T> root)
        {
            _root = root;
        }


        //override update func
        public override bool Update(T context)
        {
            return _root.Update(context);
        }
    }


    //action node
    public class Do<T> : Node<T>
    {
        public delegate bool NodeAction(T context);

        private readonly NodeAction _action;

        // pass var
        public Do(NodeAction action)
        {
            _action = action;
        }

        //override update func
        public override bool Update(T context)
        {
            return _action(context);
        }
    }


    public class Condition<T> : Node<T>
    {
        private readonly Predicate<T> _condition;
        //defines if object meets specific critiria
        //define the delegate method first when use

        public Condition(Predicate<T> condition)
        {
            _condition = condition;
        }

        public override bool Update(T context)
        {
            return _condition(context);
        }
    }

    public abstract class CompositeNode<T> : Node<T>
    {
        //composite node can have 1-n childe nodes
        protected Node<T>[] Children { get; private set; }


        //pass children nodes into readonly var
        protected CompositeNode(params Node<T>[] children)
        {
            Children = children;
        }
    }


    //define selector, derive from composite node
    public class Selector<T> : CompositeNode<T>
    {
        public Selector(params Node<T>[] children) : base(children) { }

        //if one of children return true, selector return true
        public override bool Update(T context)
        {
            foreach (var child in Children)
            {
                if (child.Update(context)) return true;
            }
            return false;
        }
    }


    //define sequence, derive from composite nodes
    public class Sequence<T> : CompositeNode<T>
    {
        public Sequence(params Node<T>[] children) : base(children) { }

        public override bool Update(T context)
        {
            foreach (var child in Children)
            {
                //if one of the children return false, the sequendce return false
                if (!child.Update(context)) return false;
            }
            return true;
        }
    }

    //define decorator node, derive from node
    public abstract class Decorator<T> : Node<T>
    {
        // the deco node can only have one child
        protected Node<T> Child { get; private set; }

        protected Decorator(Node<T> child)
        {
            Child = child;
        }
    }

    //not deco node, reverse the return form the original return
    public class Not<T> : Decorator<T>
    {
        public Not(Node<T> child) : base(child) { }

        public override bool Update(T context)
        {
            return !Child.Update(context);
        }
    }

}
