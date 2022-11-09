#include <stdio.h>
#include <iostream>
using namespace std;


int ts = 10;
int maxpi = 8;
bool check = false;
bool checkRR = false;

struct record{
	int pi;
	char name;
	int ir;
	int time;
	struct record *next,*prev;
};

struct record *tail=NULL;

struct record *rotation(struct record *head){
	struct record *p;
	p=head;
	tail->next=head;
	head->next=NULL;
	tail->prev=NULL;
	head=tail;
	tail=p;
	return head;
}


struct record *insert(struct record *head,int pi,char name,int time,int ir){
	if(head==NULL){
		head=new struct record;
		head->pi=pi;
		head->name=name;
		head->time=time;
		head->ir=ir;
		
		head->next=NULL;
		head->prev=NULL;
		tail=head;
	}
	else{
		struct record *node;
		node=new struct record;
		node->pi=pi;
		node->name=name;
		node->time=time;
		node->ir=ir;
		if(pi<=head->pi){
			node->next=head;
			head->prev=node;
			node->prev=NULL;
			head=node;
		}
		else if(pi>=tail->pi){
			tail->next=node;
			node->prev=tail;
			node->next=NULL;
			tail=node;
		}
		else{
			struct record *p=head->next;
			while(p!=NULL){
				if(pi<=p->pi){
					node->next=p;
					node->prev=p->prev;
					p->prev->next=node;
					p->prev=node;
					break;
				}
				else{
					p=p->next;
				}
			}
		}
	}
	return head;
}

void printt(struct record *head){
	struct record *tmp;
	if(head!=NULL){
		cout << "Name : "<< head->name << " " << "Priority : " << head->pi << " "<< head -> time <<" " << head->ir << " " << endl;
		tmp=head->next;
		while(tmp!=NULL){
			cout << "Name : "<< tmp->name << " " << "Priority : " << tmp->pi << " " << tmp -> time <<" " << head->ir << " "<< endl;
			tmp=tmp->next;
		}
	}
	cout << endl;
}

bool checkir(struct record *head){
	if(head -> ir < ts){
		return true;
	}
	return false;
}


struct record *more(struct record *head){
	if (checkir(head)){
		head -> time = head -> time - head -> ir;
		return (head);
	}
		head -> pi = head -> pi + 1;
		if (head->pi >= maxpi){
			head->pi = maxpi;
		}
		head -> time = head -> time - ts;
		return head;
}

struct record *RR(struct record *head){
	if (checkir(head)){
		head -> time = head -> time - head -> ir;
		head->next->time=head->next->time-head->ir;  
		return (head);
	}
	head -> pi = head -> pi + 1;
	head -> next -> pi = head -> next -> pi + 1;
	if (head -> pi > maxpi){
		head -> pi = maxpi;
		head -> next -> pi = maxpi;
	} 
	head -> ir = head -> time - ts;
	head -> next -> time = head -> next -> time -ts;
	return head;
}
 
 struct record *loopboost(struct record *head,int data){
	struct record *tmp,*p;
	int tb=0;
	int cc=0;
	cout << "Insert Time to boost Priority : ";
	cin >> tb;
	int c=0;
	int num=1;
	int flag=0;
	for(int i=0;i<data;i++){
		cout << "*****************" << endl;
		c+=head->ir;
		cout << "Time pass : " << head->ir << " Round : " << num << endl;  
		cc+=head->ir;
		if(cc<tb){
			if (head -> pi < head -> next -> pi ){
				head = more(head);
			}
			else if(head -> pi == head -> next -> pi){
				head = RR(head);
			}
		}
		else{
			head->next->pi=1;
			head=rotation(head);
			cc=cc-tb;                  
			if (head -> pi < head -> next -> pi ){
				head = more(head);
			}
			else if(head -> pi == head -> next -> pi){
				head = RR(head);
			}
		}
		//cout << "cc" << cc << endl;
		cout << "Time : " << c << endl;
		printt(head);
		num++;
	}
 }


int main(){
	struct record *head=NULL;
	head=insert(head,4,'A',200,5);
	head=insert(head,2,'B',100,5);
	cout << "Start at -vvvvvvv-" << endl;
	printt(head);
	head=loopboost(head,10);
}
