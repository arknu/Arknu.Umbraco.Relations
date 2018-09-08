angular.module("umbraco")
    .controller("Arknu.Umbraco.Relations.Controller", function ($scope, $location, arknuRelationsResource, editorState, dialogService, entityResource, navigationService, miniEditorHelper) {
        var type = $scope.model.config.relationTypeAlias;

        arknuRelationsResource.getRelations(editorState.current.id, type).then(function (response) {
            $scope.model.items = response.data;
            $scope.model.showOpenButton = true;
        });

        var dialogOptions = {
            multiPicker: true,
            entityType: "document",
            filterCssClass: "not-allowed not-published",
            startNodeId: null,
            callback: function (data) {
                if (angular.isArray(data)) {
                    _.each(data, function (item, i) {
                        $scope.add(item);
                    });
                } else {
                    $scope.clear();
                    $scope.add(data);
                }
            },
            treeAlias: "content",
            section: "content"
        };

        //dialog
        $scope.openContentPicker = function () {
            var d = dialogService.treePicker(dialogOptions);
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

        $scope.openMiniEditor = function (node) {
            miniEditorHelper.launchMiniEditor({ id: node.contentId }).then(function (updatedNode) {
                // update the node
                node.name = updatedNode.name;
                node.published = updatedNode.hasPublishedVersion;
                if (entityType !== "Member") {
                    entityResource.getUrl(updatedNode.id, entityType).then(function (data) {
                        node.url = data;
                    });
                }
            });
        };

        $scope.showNode = function (index) {
            var item = $scope.model.items[index];
            var id = item.contentId;
            var section = dialogOptions.section;

            entityResource.getPath(id, dialogOptions.entityType).then(function (path) {
                navigationService.changeSection(section);
                navigationService.showTree(section, {
                    tree: section, path: path, forceReload: false, activate: true
                });
                var routePath = section + "/" + section + "/edit/" + id.toString();
                $location.path(routePath).search("");
            });
        }
    });