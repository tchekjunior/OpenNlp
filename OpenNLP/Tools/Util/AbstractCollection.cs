﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenNLP.Tools.Util
{
    /*
      * This class provides a skeletal implementation of the <tt>Collection</tt>
      * interface, to minimize the effort required to implement this interface. <p>
      *
      * To implement an unmodifiable collection, the programmer needs only to
      * extend this class and provide implementations for the <tt>iterator</tt> and
      * <tt>size</tt> methods.  (The iterator returned by the <tt>iterator</tt>
      * method must implement <tt>hasNext</tt> and <tt>next</tt>.)<p>
      *
      * To implement a modifiable collection, the programmer must additionally
      * override this class's <tt>add</tt> method (which otherwise throws an
      * <tt>UnsupportedOperationException</tt>), and the iterator returned by the
      * <tt>iterator</tt> method must additionally implement its <tt>remove</tt>
      * method.<p>
      *
      * The programmer should generally provide a void (no argument) and
      * <tt>Collection</tt> constructor, as per the recommendation in the
      * <tt>Collection</tt> interface specification.<p>
      *
      * The documentation for each non-abstract method in this class describes its
      * implementation in detail.  Each of these methods may be overridden if
      * the collection being implemented admits a more efficient implementation.<p>
      *
      * This class is a member of the
      * <a href="{@docRoot}/../technotes/guides/collections/index.html">
      * Java Collections Framework</a>.
      *
      * @author  Josh Bloch
      * @author  Neal Gafter
      * @see Collection
      * @since 1.2
      */
     
     public abstract class AbstractCollection<E> : IEnumerable<E> {
         /**
          * Sole constructor.  (For invocation by subclass constructors, typically
          * implicit.)
          */
         protected AbstractCollection() {
         }
     
         // Query Operations
     
         /**
          * Returns an iterator over the elements contained in this collection.
          *
          * @return an iterator over the elements contained in this collection
          */
         //public abstract IEnumerator<E> GetEnumerator();
     
         //public abstract int Count();
     
         /**
          * {@inheritDoc}
          *
          * <p>This implementation returns <tt>size() == 0</tt>.
          */
         public bool Any() {
             return this.Count() != 0;
         }
     
         /**
          * {@inheritDoc}
          *
          * <p>This implementation iterates over the elements in the collection,
          * checking each element in turn for equality with the specified element.
          *
          * @throws ClassCastException   {@inheritDoc}
          * @throws NullPointerException {@inheritDoc}
          */
         public bool Contains(E o) {
             IEnumerator<E> it = GetEnumerator();
           if (o==null)
           {
               while (it.MoveNext())
               {
                   if (it.Current == null)
                   {
                       return true;
                   }
               }
           } else {
               while (it.MoveNext())
               {
                   if (o.Equals(it.Current))
                   {
                       return true;
                   }
               }
           }
           return false;
       }
   
       /**
        * {@inheritDoc}
        *
        * <p>This implementation returns an array containing all the elements
        * returned by this collection's iterator, in the same order, stored in
        * consecutive elements of the array, starting with index {@code 0}.
        * The length of the returned array is equal to the number of elements
        * returned by the iterator, even if the size of this collection changes
        * during iteration, as might happen if the collection permits
        * concurrent modification during iteration.  The {@code size} method is
        * called only as an optimization hint; the correct result is returned
        * even if the iterator returns a different number of elements.
        *
        * <p>This method is equivalent to:
        *
        *  <pre> {@code
        * List<E> list = new ArrayList<E>(size());
        * for (E e : this)
        *     list.add(e);
        * return list.toArray();
        * }</pre>
        */
       public E[] ToArray()
       {
           var list = new List<E>();
           var it = GetEnumerator();
           while (it.MoveNext())
           {
               list.Add(it.Current);
           }
           return list.ToArray();
           // Estimate size of array; be prepared to see more or fewer elements
           /*E[] r = new E[Count()];
           var it = iterator();
           for (int i = 0; i < r.Length; i++) {
               if (! it.hasNext()) // fewer elements than expected
                   return Arrays.copyOf(r, i);
               r[i] = it.next();
           }
           return it.hasNext() ? finishToArray(r, it) : r;*/
       }
   
       /**
        * {@inheritDoc}
        *
        * <p>This implementation returns an array containing all the elements
        * returned by this collection's iterator in the same order, stored in
        * consecutive elements of the array, starting with index {@code 0}.
        * If the number of elements returned by the iterator is too large to
        * fit into the specified array, then the elements are returned in a
        * newly allocated array with length equal to the number of elements
        * returned by the iterator, even if the size of this collection
        * changes during iteration, as might happen if the collection permits
        * concurrent modification during iteration.  The {@code size} method is
        * called only as an optimization hint; the correct result is returned
        * even if the iterator returns a different number of elements.
        *
        * <p>This method is equivalent to:
        *
        *  <pre> {@code
        * List<E> list = new ArrayList<E>(size());
        * for (E e : this)
        *     list.add(e);
        * return list.toArray(a);
        * }</pre>
        *
        * @throws ArrayStoreException  {@inheritDoc}
        * @throws NullPointerException {@inheritDoc}
        */
       /*public <T> T[] toArray(T[] a) {
           // Estimate size of array; be prepared to see more or fewer elements
           int size = size();
           T[] r = a.Length >= size ? a :
                     (T[])java.lang.reflect.Array
                     .newInstance(a.getClass().getComponentType(), size);
           Iterator<E> it = iterator();
   
           for (int i = 0; i < r.Length; i++) {
               if (! it.hasNext()) { // fewer elements than expected
                   if (a != r)
                       return Arrays.copyOf(r, i);
                   r[i] = null; // null-terminate
                   return r;
               }
               r[i] = (T)it.next();
           }
           return it.hasNext() ? finishToArray(r, it) : r;
       }*/
   
       /**
        * The maximum size of array to allocate.
        * Some VMs reserve some header words in an array.
        * Attempts to allocate larger arrays may result in
        * OutOfMemoryError: Requested array size exceeds VM limit
        */
       private static readonly int MAX_ARRAY_SIZE = int.MaxValue - 8;
   
       /**
        * Reallocates the array being used within toArray when the iterator
        * returned more elements than expected, and finishes filling it from
        * the iterator.
        *
        * @param r the array, replete with previously stored elements
        * @param it the in-progress iterator over this collection
        * @return array containing the elements in the given array, plus any
        *         further elements returned by the iterator, trimmed to size
        */
       /*private static <T> T[] finishToArray(T[] r, Iterator<?> it) {
           int i = r.Length;
           while (it.hasNext()) {
               int cap = r.Length;
               if (i == cap) {
                   int newCap = cap + (cap >> 1) + 1;
                   // overflow-conscious code
                   if (newCap - MAX_ARRAY_SIZE > 0)
                       newCap = hugeCapacity(cap + 1);
                   r = Arrays.copyOf(r, newCap);
               }
               r[i++] = (T)it.next();
           }
           // trim if overallocated
           return (i == r.Length) ? r : Arrays.copyOf(r, i);
       }*/
   
       /*private static int hugeCapacity(int minCapacity) {
           if (minCapacity < 0) // overflow
               throw new OutOfMemoryException("Required array size too large");
           return (minCapacity > MAX_ARRAY_SIZE) ? int.MaxValue: MAX_ARRAY_SIZE;
       }*/
   
       // Modification Operations
   
       /**
        * {@inheritDoc}
        *
        * <p>This implementation always throws an
        * <tt>UnsupportedOperationException</tt>.
        *
        * @throws UnsupportedOperationException {@inheritDoc}
        * @throws ClassCastException            {@inheritDoc}
        * @throws NullPointerException          {@inheritDoc}
        * @throws IllegalArgumentException      {@inheritDoc}
        * @throws IllegalStateException         {@inheritDoc}
        */
       public void Add(E e) {
           throw new InvalidOperationException();
       }
   
       /**
        * {@inheritDoc}
        *
        * <p>This implementation iterates over the collection looking for the
        * specified element.  If it finds the element, it removes the element
        * from the collection using the iterator's remove method.
        *
        * <p>Note that this implementation throws an
        * <tt>UnsupportedOperationException</tt> if the iterator returned by this
        * collection's iterator method does not implement the <tt>remove</tt>
        * method and this collection contains the specified object.
        *
        * @throws UnsupportedOperationException {@inheritDoc}
        * @throws ClassCastException            {@inheritDoc}
        * @throws NullPointerException          {@inheritDoc}
        */
       /*public bool Remove(E o) {
           IEnumerator<E> it = iterator();
           if (o==null) {
               while (it.MoveNext()) {
                   if (it.Current==null) {
                       it.remove();
                       return true;
                   }
               }
           } else {
               while (it.hasNext()) {
                   if (o.equals(it.next())) {
                       it.remove();
                       return true;
                   }
               }
           }
           return false;
       }*/
   
   
       // Bulk Operations
   
       /**
        * {@inheritDoc}
        *
        * <p>This implementation iterates over the specified collection,
        * checking each element returned by the iterator in turn to see
        * if it's contained in this collection.  If all elements are so
        * contained <tt>true</tt> is returned, otherwise <tt>false</tt>.
        *
        * @throws ClassCastException            {@inheritDoc}
        * @throws NullPointerException          {@inheritDoc}
        * @see #contains(Object)
        */
       public bool ContainsAll(ICollection<E> c) {
           foreach (E e in c)
           {
               if (!Contains(e))
               {
                   return false;
               }
           }
           return true;
       }
   
       /**
        * {@inheritDoc}
        *
        * <p>This implementation iterates over the specified collection, and adds
        * each object returned by the iterator to this collection, in turn.
        *
        * <p>Note that this implementation will throw an
        * <tt>UnsupportedOperationException</tt> unless <tt>add</tt> is
        * overridden (assuming the specified collection is non-empty).
        *
        * @throws UnsupportedOperationException {@inheritDoc}
        * @throws ClassCastException            {@inheritDoc}
        * @throws NullPointerException          {@inheritDoc}
        * @throws IllegalArgumentException      {@inheritDoc}
        * @throws IllegalStateException         {@inheritDoc}
        *
        * @see #add(Object)
        */
       public void AddAll(ICollection<E> c) {
           foreach (E e in c)
           {
               Add(e);
           }
       }
   
       /**
        * {@inheritDoc}
        *
        * <p>This implementation iterates over this collection, checking each
        * element returned by the iterator in turn to see if it's contained
        * in the specified collection.  If it's so contained, it's removed from
        * this collection with the iterator's <tt>remove</tt> method.
        *
        * <p>Note that this implementation will throw an
        * <tt>UnsupportedOperationException</tt> if the iterator returned by the
        * <tt>iterator</tt> method does not implement the <tt>remove</tt> method
        * and this collection contains one or more elements in common with the
        * specified collection.
        *
        * @throws UnsupportedOperationException {@inheritDoc}
        * @throws ClassCastException            {@inheritDoc}
        * @throws NullPointerException          {@inheritDoc}
        *
        * @see #remove(Object)
        * @see #contains(Object)
        */
       /*public bool RemoveAll(ICollection<E> c) {
           bool modified = false;
           Iterator<?> it = iterator();
           while (it.hasNext()) {
               if (c.contains(it.next())) {
                   it.remove();
                   modified = true;
               }
           }
           return modified;
       }*/
   
       /**
        * {@inheritDoc}
        *
        * <p>This implementation iterates over this collection, checking each
        * element returned by the iterator in turn to see if it's contained
        * in the specified collection.  If it's not so contained, it's removed
        * from this collection with the iterator's <tt>remove</tt> method.
        *
        * <p>Note that this implementation will throw an
        * <tt>UnsupportedOperationException</tt> if the iterator returned by the
        * <tt>iterator</tt> method does not implement the <tt>remove</tt> method
        * and this collection contains one or more elements not present in the
        * specified collection.
        *
        * @throws UnsupportedOperationException {@inheritDoc}
        * @throws ClassCastException            {@inheritDoc}
        * @throws NullPointerException          {@inheritDoc}
        *
        * @see #remove(Object)
        * @see #contains(Object)
        */
       /*public bool retainAll(ICollection<E> c) {
           bool modified = false;
           Iterator<E> it = iterator();
           while (it.hasNext()) {
               if (!c.contains(it.next())) {
                   it.remove();
                   modified = true;
               }
           }
           return modified;
       }*/
   
       /**
        * {@inheritDoc}
        *
        * <p>This implementation iterates over this collection, removing each
        * element using the <tt>Iterator.remove</tt> operation.  Most
        * implementations will probably choose to override this method for
        * efficiency.
        *
        * <p>Note that this implementation will throw an
        * <tt>UnsupportedOperationException</tt> if the iterator returned by this
        * collection's <tt>iterator</tt> method does not implement the
        * <tt>remove</tt> method and this collection is non-empty.
        *
        * @throws UnsupportedOperationException {@inheritDoc}
        */
       /*public void Clear() {
           Iterator<E> it = iterator();
           while (it.hasNext()) {
               it.next();
               it.remove();
           }
       }*/
   
   
       //  String conversion
   
       /**
        * Returns a string representation of this collection.  The string
        * representation consists of a list of the collection's elements in the
        * order they are returned by its iterator, enclosed in square brackets
        * (<tt>"[]"</tt>).  Adjacent elements are separated by the characters
        * <tt>", "</tt> (comma and space).  Elements are converted to strings as
        * by {@link String#valueOf(Object)}.
        *
        * @return a string representation of this collection
        */
         public abstract IEnumerator<E> GetEnumerator();

         public override String ToString() {
           IEnumerator<E> it = GetEnumerator();
           StringBuilder sb = new StringBuilder();
           sb.Append('[');
           while (it.MoveNext())
           {
               E e = it.Current;
               sb.Append(e);
               sb.Append(',').Append(' ');
           }
           sb = sb.Remove(sb.Length - 2, 2);
           return sb.Append(']').ToString();
           /*if (! it.hasNext())
               return "[]";
   
           StringBuilder sb = new StringBuilder();
           sb.Append('[');
           for (;;) {
               E e = it.next();
               sb.Append(e == this ? "(this Collection)" : e);
               if (! it.hasNext())
                   return sb.Append(']').ToString();
               sb.Append(',').Append(' ');
           }*/
       }

         IEnumerator IEnumerable.GetEnumerator()
         {
             return GetEnumerator();
         }
     }
}