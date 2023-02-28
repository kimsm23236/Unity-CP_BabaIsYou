# Unity-CP_BabaIsYou
Unity Portfolio Copy Project Baba Is You         

2023-02-13~2023-02-16 / v0.0.1 / Project setup, Asset work, Grid, Object, Json based object data load     
2023-02-16 / v0.0.2 / Json based stage data loading, Stage load         
2023-02-16 / v0.0.3 / Rule Making System Base Setup   
2023-02-17 / v0.0.4 / Working Rule Making System -> DFS Rule Checking   
2023-02-20 / v0.0.5 / Grid Based Movement, Attribute : You, Win    
2023-02-20 / v0.0.6 / Attribute : Push  
2023-02-21 / v0.1.0 / End Prototype Version, Attribute Stop, 0 Stage   
2023-02-21 / v0.1.1 / Move Record and Undo Base       
2023-02-22 / v0.1.2 / Object Color Setup, Undo working       
2023-02-23 / v0.1.3 / Undo Prototype Ended      
2023-02-23 / v0.1.4 / Object Tiling     
2023-02-23 / v0.1.5 / Tiling Prototype Ended 
Issue.           
타일링을 위해 스크립트를 작성하였는데 타일링 오브젝트가 많을 시 게임이 현저히 느려지는 현상 발생              
프로파일러 툴을 사용하여 원인이 되는 함수를 발견        
Solution.         
타일링 검사를 코루틴을 사용하여 실행하였는데 매 프레임마다 해당 함수를 실행하였고 타일링 오브젝트가 많을수록 프레임 드랍이 더 심해졌었음 이를 필요한 순간에만 실행하는 것으로 어느정도 해결하였으나 좀 더 개선할수 있을 것 같음        

2023-02-24 / v0.1.6 / Refactoring, Solve Framedrop Issue  
Refactoring.    
rf.1.           
많은 기능을 담당하던 ObjectProperty 컴포넌트를 분할     
기존에는 오브젝트 정보에 더해 이동, 위치정보 등 이동 관련 기능까지 담당하였는데 이러한 이동에 관한 모든 멤버들을 ObjectMovement 라는 새로이 작성한 스크립트에 넣어주어 책임 및 기능을 분할하였음.       
rf.2.               
규칙을 RuleMakingSystem에서 ObjectProperty의 배열로 관리하고 있었으나 이러한 방식이 규칙을 관리하고 다루는데 있어 불편하다고 생각되어 담당하던 규칙 적용 및 규칙 해제 등을 Rule 클래스를 따로 만들어서 담당하게 하고 Rule을 RuleMakingSystem에서 관리하게끔 하여 객체지향성을 좀더 높여보았음        

2023-02-27 / v0.1.7 / Level Editor Tool Base     
2023-02-28 / v0.1.8 / Working Level Editor Tool                  
              

