<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TakeQuiz.aspx.cs" Inherits="MyBahasa.TakeQuiz" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <title>Take Quiz - MyBahasa</title>
    <style>
        body { font-family: 'Segoe UI', sans-serif; background:#f9fafb; margin:0; }
        .container { max-width: 900px; margin: 50px auto; background:#fff; padding:40px; border-radius:10px; box-shadow:0 4px 10px rgba(0,0,0,.1); }
        h2 { text-align:center; color:#007b83; margin-bottom:10px; }
        h4 { margin:25px 0 10px; color:#333; }
        .question { margin-bottom:30px; padding-bottom:15px; border-bottom:1px solid #eee; }
        .choices { margin-left: 10px; }
        .choice { display:flex; align-items:center; gap:8px; margin:6px 0; cursor:pointer; font-size:15px; color:#333; padding: 8px; border-radius: 5px; transition: background-color 0.2s; }
        .choice:hover { background-color: #f5f5f5; }
        .btn { background:#007b83; color:#fff; border:0; padding:12px 20px; border-radius:8px; font-size:16px; cursor:pointer; margin-top: 20px; }
        .btn:hover { background:#00666e; }
        .btn:disabled { background:#ccc; cursor:not-allowed; }
        .message { text-align:center; margin-top:25px; font-size:18px; color:#007b83; font-weight:600; }
        
        input[type="radio"] {
            margin: 0;
            cursor: pointer;
            transform: scale(1.2);
        }
        
        .quiz-info {
            text-align: center;
            color: #666;
            margin-bottom: 20px;
            font-size: 14px;
        }
        
        .required { color: red; }
        
        .question-error {
            border-left: 3px solid red !important;
            padding-left: 10px !important;
        }
        
        .choice.selected {
            background-color: #e8f4f8;
        }
    </style>
</head>
<body>
<form id="form1" runat="server">
    <div class="container">
        <h2><asp:Label ID="lblLessonTitle" runat="server" /></h2>
        <div class="quiz-info">Please select one answer for each question. All questions are required.</div>
        <hr />

        <asp:Panel ID="pnlQuiz" runat="server">
            <asp:Repeater ID="rptQuestions" runat="server" OnItemDataBound="rptQuestions_ItemDataBound">
                <ItemTemplate>
                    <div class="question" id="question_<%# Eval("QuestionID") %>">
                        <h4>Q<%# Container.ItemIndex + 1 %>: <%# Eval("StemText") %> <span class="required">*</span></h4>

                        <!-- store question id for submit -->
                        <asp:HiddenField ID="hfQuestionId" runat="server" Value='<%# Eval("QuestionID") %>' />

                        <div class="choices">
                            <asp:Repeater ID="rptChoices" runat="server" OnItemDataBound="rptChoices_ItemDataBound">
                                <ItemTemplate>
                                    <label class="choice">
                                        <!-- real ASP radio (single answer enforced via GroupName) -->
                                        <asp:RadioButton ID="rbChoice" runat="server" GroupName='<%# "QuestionGroup_" + Eval("QuestionID") %>' />
                                        <!-- store choice id beside the radio -->
                                        <asp:HiddenField ID="hfChoiceId" runat="server" Value='<%# Eval("ChoiceID") %>' />
                                        <asp:Literal ID="litChoiceText" runat="server" Text='<%# Eval("ChoiceText") %>'></asp:Literal>
                                    </label>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>

            <asp:Button ID="btnSubmit" runat="server" Text="Submit Quiz" CssClass="btn" OnClick="btnSubmit_Click" />
        </asp:Panel>

        <asp:Label ID="lblMessage" runat="server" CssClass="message" Visible="false"></asp:Label>
    </div>
</form>

<script type="text/javascript">
    function validateQuiz() {
        var allAnswered = true;
        var questions = document.querySelectorAll('.question');
        
        for (var i = 0; i < questions.length; i++) {
            var question = questions[i];
            var radioButtons = question.querySelectorAll('input[type="radio"]');
            var answered = false;
            
            for (var j = 0; j < radioButtons.length; j++) {
                if (radioButtons[j].checked) {
                    answered = true;
                    break;
                }
            }
            
            if (!answered) {
                allAnswered = false;
                question.classList.add('question-error');
            } else {
                question.classList.remove('question-error');
            }
        }
        
        if (!allAnswered) {
            alert('Please answer all questions before submitting the quiz.');
            return false;
        }
        
        return true;
    }
    
    var submitButton = document.getElementById('<%= btnSubmit.ClientID %>');
    if (submitButton) {
        submitButton.onclick = function () {
            return validateQuiz();
        };
    }

    // Enhanced selection handling
    document.addEventListener('click', function (e) {
        if (e.target.type === 'radio') {
            var label = e.target.closest('.choice');
            if (label) {
                var question = label.closest('.question');
                var allChoices = question.querySelectorAll('.choice');

                // Remove highlight from all choices in this question
                allChoices.forEach(function (choice) {
                    choice.classList.remove('selected');
                });

                // Highlight selected choice
                label.classList.add('selected');
                question.classList.remove('question-error');
            }
        }

        // Also handle clicks on the label text (not just the radio)
        if (e.target.classList.contains('choice') || e.target.closest('.choice')) {
            var choiceLabel = e.target.classList.contains('choice') ? e.target : e.target.closest('.choice');
            if (choiceLabel) {
                var radio = choiceLabel.querySelector('input[type="radio"]');
                if (radio) {
                    radio.checked = true;

                    var question = choiceLabel.closest('.question');
                    var allChoices = question.querySelectorAll('.choice');

                    // Remove highlight from all choices in this question
                    allChoices.forEach(function (choice) {
                        choice.classList.remove('selected');
                    });

                    // Highlight selected choice
                    choiceLabel.classList.add('selected');
                    question.classList.remove('question-error');
                }
            }
        }
    });

    // Initialize any previously selected answers on page load
    document.addEventListener('DOMContentLoaded', function () {
        var questions = document.querySelectorAll('.question');
        questions.forEach(function (question) {
            var selectedRadio = question.querySelector('input[type="radio"]:checked');
            if (selectedRadio) {
                var selectedLabel = selectedRadio.closest('.choice');
                if (selectedLabel) {
                    selectedLabel.classList.add('selected');
                }
            }
        });
    });
</script>
</body>
</html>