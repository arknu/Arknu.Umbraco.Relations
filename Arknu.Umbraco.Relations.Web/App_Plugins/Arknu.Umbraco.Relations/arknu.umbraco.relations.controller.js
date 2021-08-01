angular.module("umbraco")
    .controller("Arknu.Umbraco.Relations.Controller", function ($scope, $location, arknuRelationsResource, editorState, editorService, entityResource, navigationService) {
        var type = $scope.model.config.relationTypeAlias;

        arknuRelationsResource.getRelations(editorState.current.id, type).then(function (response) {
            $scope.model.items = response.data;
            $scope.model.showOpenButton = true;
        });

        var dialogOptions = {
            multiPicker: true,
            filterCssClass: "not-allowed not-published",
            submit: function (data) {
                var selection = data.selection;
                _.each(selection, function (item, i) {
                    $scope.add(item);
                });

                editorService.close();
            },
            close: function () {
                editorService.close();
            }
        };

        //dialog
        $scope.openContentPicker = function () {
            var d = editorService.contentPicker(dialogOptions);
        };

        $scope.remove = function (index) {
            var item = $scope.model.items[index];
            arknuRelationsResource.deleteRelation(item.relationId).then(function (response) {
                $scope.model.items.splice(index, 1);
            });

        };

        $scope.add = function (item) {
            arknuRelationsResource.saveRelation(editorState.current.id, item.id, type).then(function (response) {
                $scope.model.items.push(response.data);
            });


        };

        $scope.showNode = function (item) {
            var id = item.contentId;

            var editor = {
                id: id,
                submit: function(model) {
                    editorService.close();
                },
                close: function() {
                    editorService.close();
                }
            };

            editorService.contentEditor(editor);
        }
    });