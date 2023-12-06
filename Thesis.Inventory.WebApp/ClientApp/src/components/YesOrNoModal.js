import { ContextualMenu, DefaultButton, Dialog, DialogFooter, DialogType, IconButton, PrimaryButton, Toggle, getTheme, hiddenContentStyle, mergeStyles } from "@fluentui/react"
import { reportTypeError } from "ajv/dist/compile/validate/dataType"
import react, { useId, useMemo, useState } from "react";
import { Label } from "reactstrap"

const dialogStyles = { main: { maxWidth: 450 } };
const dragOptions = {
    moveMenuItemText: 'Move',
    closeMenuItemText: 'Close',
    menu: ContextualMenu,
    keepInBounds: true,
};
const screenReaderOnly = mergeStyles(hiddenContentStyle);


const YesNo = ({ onAnswer, message, hideModal }) => {

    

    const [isDraggable, { toggle: toggleIsDraggable }] = useState(false);
    const labelId: string = useId('dialogLabel');
    const subTextId: string = useId('subTextLabel');

    const dialogContentProps = {
        type: DialogType.normal,
        title: 'Inventory',
        closeButtonAriaLabel: 'Close',
        subText: message,
    };

    const modalProps = useMemo(
        () => ({
            titleAriaId: labelId,
            subtitleAriaId: subTextId,
            isBlocking: false,
            styles: dialogStyles,
            dragOptions: isDraggable ? dragOptions : undefined,
        }),
        [isDraggable, labelId, subTextId],
    );

    return (
        <>
            <label id={labelId} className={screenReaderOnly}>
                My sample label
            </label>
            <label id={subTextId} className={screenReaderOnly}>
                My sample description
            </label>

            <Dialog
                hidden={hideModal}
                onDismiss={() =>  onAnswer('No')}
                dialogContentProps={dialogContentProps}
                modalProps={modalProps}
            >
                <DialogFooter>
                    <PrimaryButton onClick={() => {
                        onAnswer('Yes');
                    }} text="Yes" />
                    <DefaultButton onClick={() => {
                        onAnswer('No');
                    }} text="No" />
                </DialogFooter>
            </Dialog>
        </>
    );
}

const cancelIcon: IIconProps = { iconName: 'Cancel' };
const theme = getTheme();
const iconButtonStyles: Partial<IButtonStyles> = {
    root: {
        color: theme.palette.neutralPrimary,
        marginLeft: 'auto',
        marginTop: '4px',
        marginRight: '2px',
    },
    rootHovered: {
        color: theme.palette.neutralDark,
    },
};
export default YesNo;